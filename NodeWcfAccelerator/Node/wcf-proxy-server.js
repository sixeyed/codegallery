
// node wcf-proxy-server.js

// Node proxies any calls made to :8010. Compare WCF and Node with:
// http://127.0.0.1/WcfSampleService/PersonService.svc/rest/fetch/1
// http://127.0.0.1:8010/WcfSampleService/PersonService.svc/rest/fetch/1

var port = 8010;
var host = '127.0.0.1';

var http = require('http');
var url = require('url');
var fs = require('fs');
var path = require('path');
var Memcached = require('./memcached');
var memcached = new Memcached('127.0.0.1:11211');

//create local cache store - this is the only synchronous call:
var cachePath = 'caches';
if (!path.existsSync(cachePath)) {
    fs.mkdirSync('caches');
}

function setETag(url, eTag) {
    cache[url] = eTag;
}

function getProxyETag(url) {
    var eTag = cache[url];
    if (!eTag) {
        eTag = 'eTag'
    }
    return eTag;
}

function resError(res) {
    res.writeHead(500, { 'Content-Type': 'text/plain' });
    res.end('Internal Server Error');
}

function res304(res, eTag) {
    res.writeHead(304, {
        'ETag': eTag
    });
    res.end();
    console.log('Returned 304');
}

function res200FromCache(res, cacheFilename, eTag) {
    //read from the cache:
    console.log('Reading cache file: ' + cacheFilename);
    fs.readFile(cacheFilename, 'utf-8', function (error, content) {
        if (error) {
            console.log(error);
            resError(res);
        }
        else {
            fs.readFile(geHeaderPath(eTag), function (error, data) {
                if (!error) {
                    res.writeHead(200, JSON.parse(data));                    
                }
                res.end(content);
                console.log('Returned 200. Served: ' + cacheFilename);
            });
        }
    });
}

function getRootFilePath(eTag) {
    //we expect a GUID wrapped in double-quotes, e.g. "6eaad383-9eb3-4481-9706-322b77234d5"
    //it's unique so we can use it as a filename:    
    return '.\\' + cachePath + '\\' + eTag.replace(/\"/g, '');
}

function getCachePath(eTag) {
    return getRootFilePath(eTag) + '.body';
}

function geHeaderPath(eTag) {
    return getRootFilePath(eTag) + '.head';
}

http.createServer(function (req, res) {
    try {

        var clientETag = req.headers['if-none-match'];
        var relativePath = url.parse(req.url).path;
        console.log('Serving path: ' + relativePath);

        memcached.get(relativePath, function (error, answer) {
            if (!error) {
                var serverETag = answer;
            }

            console.log('Using client eTag: ' + clientETag);
            console.log('Using server eTag: ' + serverETag);

            //if the client has the same version cached, return 304:
            if (clientETag == serverETag) {
                console.log('Client eTag matches cached server eTag');
                res304(res, serverETag);
            }
            else {
                //check that the cache exists:
                var cacheFilename = getCachePath(serverETag);
                path.exists(cacheFilename, function (exists) {
                    if (exists) {
                        res200FromCache(res, cacheFilename, serverETag);
                    }
                    else {
                        console.log('Cache file not found: ' + cacheFilename);
                        var options = {
                            host: 'localhost',
                            port: 80,
                            path: relativePath,
                            method: 'GET'
                        };

                        var proxyReq = http.request(options, function (proxyRes) {
                            //we expect the service to update the cached eTag, so we rely on the return value:
                            serverETag = proxyRes.headers['etag'];
                            console.log("Called service, response code: " + proxyRes.statusCode + ' eTag: ' + serverETag);
                            cacheFilename = getCachePath(serverETag);                            

                            if (proxyRes.statusCode == 200) {
                                res.writeHead(200, proxyRes.headers);
                                //cache the headers:
                                fs.writeFile(geHeaderPath(serverETag), JSON.stringify(proxyRes.headers));
                                //create a cache file for the response:
                                var stream = fs.createWriteStream(cacheFilename, { flags: 'w' });                                
                            }
                            proxyRes.on('data', function (chunk) {
                                console.log('Proxy data fired, res: ' + res + ', stream: ' + stream);
                                if (res) {
                                    res.write(chunk);
                                }
                                if (stream) {
                                    stream.write(chunk);
                                }
                            });
                            proxyRes.on('end', function () {
                                console.log('Proxy end fired, res: ' + res + ', stream: ' + stream);
                                if (res && proxyRes.statusCode != 304) {
                                    res.end();
                                }
                                if (stream) {
                                    stream.end();
                                }
                            });
                        });

                        proxyReq.on('error', function (e) {
                            console.log("Got error: " + e.message);
                            resError(res);
                        });

                        proxyReq.end();
                    }
                });
            }
        });
    } catch (err) {
        resError(res);
    }

}).listen(port, host);

console.log('Server running at http://127.0.0.1:8010/');