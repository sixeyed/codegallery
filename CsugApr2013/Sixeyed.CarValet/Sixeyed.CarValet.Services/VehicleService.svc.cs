using Sixeyed.CarValet.Entities;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Sixeyed.CarValet.Services
{
    [ServiceContract]
    public class VehicleService
    {
        [OperationContract]
        [WebGet(UriTemplate = "makes", ResponseFormat = WebMessageFormat.Json)]
        public IEnumerable<Entities.Make> GetMakes()
        {
            var makes = new List<Entities.Make>();
            using (var context = new VehicleEntities())
            {
                makes.AddRange(context.Makes.Select(x => new Entities.Make()
                {
                    Code = x.MakeCode,
                    Name = x.MakeName
                }));
            }
            return makes;
        }

        [OperationContract]
        [WebGet(UriTemplate = "makes/xml", ResponseFormat = WebMessageFormat.Xml)]
        public IEnumerable<Entities.Make> GetMakesXml()
        {
            return GetMakes();
        }

        [OperationContract]
        [WebGet(UriTemplate = "models/make/{makeCode}", ResponseFormat = WebMessageFormat.Json)]
        public IEnumerable<Entities.Model> GetModels(string makeCode)
        {
            var models = new List<Entities.Model>();
            using (var context = new VehicleEntities())
            {
                models.AddRange(context.Models.Where(x => x.MakeCode == makeCode)
                                              .Select(x => new Entities.Model()
                                              {
                                                  Code = x.MakeCode,
                                                  Name = x.ModelName,
                                                  MakeCode = x.MakeCode
                                              }));
            }
            return models;
        }
    }
}
