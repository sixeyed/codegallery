var ApiClient;
(function (ApiClient) {
    var Vehicles = (function () {
        function Vehicles() { }
        Vehicles.baseUrl = 'http://localhost/Sixeyed.CarValet.Api/api/vehicle';
        Vehicles.prototype.getModels = function (makeCode) {
            return 'some other JSON';
        };
        Vehicles.prototype.getVehicle = function (makeCode, modelCode, successCallback) {
            var data = {
                "makeCode": makeCode,
                "modelCode": modelCode
            };
            $.getJSON(Vehicles.baseUrl, data, successCallback);
        };
        return Vehicles;
    })();
    ApiClient.Vehicles = Vehicles;    
})(ApiClient || (ApiClient = {}));
var client = new ApiClient.Vehicles();
var models = client.getModels('rover');
