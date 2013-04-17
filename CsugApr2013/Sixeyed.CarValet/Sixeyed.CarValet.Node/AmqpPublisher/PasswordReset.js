
//npm install amqp -g

var http = require('http');
var url = require('url');
var amqp = require('./amqp/amqp');

function endsWith(str, suffix) {
    return str.indexOf(suffix, str.length - suffix.length) !== -1;
}

amqpUrl = 'amqp://localhost:5672';

console.log("Starting ... AMQP URL: " + amqpUrl);
var conn = amqp.createConnection({ url: amqpUrl }, { defaultExchangeName: "" });
conn.on('ready', setup);
conn.on('error', function (e) {
    console.log('error: ' + JSON.stringify(e));
});

function setup() {
    var queue = conn.queue('password_reset', { durable: true, exclusive: false, autoDelete: false },
    function () {
        queue.bind('#');
    });
    queue.on('queueBindOk', function () { httpServer(conn); });
}

function httpServer(conn) {
    http.createServer(function (request, response) {
        output = null;
        requestUrl = url.parse(request.url, true);
        if (endsWith(requestUrl.pathname, '/passwordreset')) {
            emailAddress = requestUrl.query.emailAddress;
            console.log('Received password reset request with email address: ' + emailAddress);
            message = JSON.stringify({ EmailAddress: emailAddress });
            conn.publish('password_reset', message);
        }
        response.writeHead(200, { 'Content-Type': 'application/json' });
        response.end(message);
    }).listen(8012);
    console.log('PasswordReset listening at http://127.0.0.1:8012/');
}

