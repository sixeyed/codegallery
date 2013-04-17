using MongoDB.Driver;
using Sixeyed.CarValet.Api.Models;
using Sixeyed.CarValet.Entities;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace Sixeyed.CarValet.Api.Controllers
{
    public class VehicleController : ApiController
    {
        public VehicleModel GetByMakeAndModelCode(string makeCode, string modelCode)
        {
            var model = GetFromCache(makeCode, modelCode);
            if (model == null)
            {
                Vehicle vehicle = null;
                using (var context = new VehicleEntities())
                {
                    vehicle = context.Vehicles.Include("Model")
                        .Include("Model.Make")
                        .FirstOrDefault(x => x.Model.ModelCode == modelCode && x.Model.MakeCode == makeCode);
                }
                if (vehicle == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                model = new VehicleModel()
                {
                    ImageUrl = vehicle.ImageUrl,
                    Make = vehicle.Model.Make.MakeName,
                    Model = vehicle.Model.ModelName,
                    ProducedFromUtc = vehicle.ProducedFromUtc,
                    ProducedToUtc = vehicle.ProducedToUtc
                };
                PutToCache(model, makeCode, modelCode);
            }
            return model;
        }

        #region Mongo caching

        private static VehicleModel GetFromCache(string makeCode, string modelCode)
        {
            var db = GetDatabase();
            var collection = db.GetCollection<CachedVehicleModel>("vehicles");
            var cached = collection.FindAll().FirstOrDefault(x => x.CacheKey == CachedVehicleModel.GetCacheKey(makeCode, modelCode));
            VehicleModel model = null;
            if (cached != null)
            {
                model = cached.Model;
            }
            return model;
        }

        private static void PutToCache(VehicleModel model, string makeCode, string modelCode)
        {
            var cached = new CachedVehicleModel()
            {
                Model = model,
                CacheKey = CachedVehicleModel.GetCacheKey(makeCode, modelCode)
            };
            var db = GetDatabase();
            var collection = db.GetCollection<CachedVehicleModel>("vehicles");
            collection.Save(cached);
        }
 
        private static MongoDatabase GetDatabase()
        {
            var connectionString = "YOUR-CONNECTION-STRING";
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var db = server.GetDatabase("YOUR-DB-NAME");
            return db;
        }

        #endregion
    }
}