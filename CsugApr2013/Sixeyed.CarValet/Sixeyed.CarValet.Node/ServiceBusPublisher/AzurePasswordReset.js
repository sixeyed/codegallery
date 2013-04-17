
//npm install azure -g

var http = require('http');
var url = require('url');
var azure = require('azure');

function endsWith(str, suffix) {
    return str.indexOf(suffix, str.length - suffix.length) !== -1;
}

console.log("Starting ...Namespace: " + process.env.AZURE_SERVICEBUS_NAMESPACE);
var serviceBusService = azure.createServiceBusService();
serviceBusService.createQueueIfNotExists('password_reset', function (error) {
    if (error) {
        console.log(JSON.stringify(error));
    }
});
    
var port = process.env.PORT || 8022;

    http.createServer(function (request, response) {
        output = null;
        requestUrl = url.parse(request.url, true);
        if (endsWith(requestUrl.pathname, '/passwordreset')) {
            emailAddress = requestUrl.query.emailAddress;
            console.log('Received password reset request with email address: ' + emailAddress);
            var body = JSON.stringify({ EmailAddress: emailAddress });
            var message = {body: body};
            var serviceBusService = azure.createServiceBusService();
            serviceBusService.sendQueueMessage('password_reset', message, function (error) {
                if (error) {
                    console.log(JSON.stringify(error));
                }
            });
        }
        response.writeHead(200, { 'Content-Type': 'application/json' });
        response.end(body);
    }).listen(port);
    console.log('AzurePasswordReset listening on ' + port);


