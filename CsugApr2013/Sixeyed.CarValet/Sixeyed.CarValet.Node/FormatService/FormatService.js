
var http = require('http');
var url = require('url');

function reverse(input) {    
    split = input.split("");
    reversedSplit = split.reverse();
    return reversedSplit.join("");
}

function endsWith(str, suffix) {
    return str.indexOf(suffix, str.length - suffix.length) !== -1;
}

var port = process.env.PORT || 8010;

http.createServer(function (request, response) {   
    output = null;
    requestUrl = url.parse(request.url, true);  
    if (endsWith(requestUrl.pathname, '/reverse')) {
        input = requestUrl.query.in;    
        console.log('Received reverse request with input: ' + input);
        output = { In: input, Out: reverse(input) };
    }
    response.writeHead(200, { 'Content-Type': 'application/json' });
    response.end(JSON.stringify(output));
}).listen(port);

console.log('FormatService listening on ' + port);