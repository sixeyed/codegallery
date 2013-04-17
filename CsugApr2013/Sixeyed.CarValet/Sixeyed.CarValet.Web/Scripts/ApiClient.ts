/// <reference path="jquery.d.ts" />

module ApiClient {

    export class Vehicles {

        static baseUrl = 'http://localhost/Sixeyed.CarValet.Api/api/vehicle';

        getModels(makeCode: string) {
            return 'some other JSON';
        };

        getVehicle(makeCode: string, modelCode: string, successCallback) {
            var data = { "makeCode": makeCode, "modelCode": modelCode };
            $.getJSON(Vehicles.baseUrl, data, successCallback);
        };
    }
}

var client: ApiClient.Vehicles = new ApiClient.Vehicles;
var models = client.getModels('rover');