using System;
using System.Collections.Generic;
using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;
using CloudServiceBus.Core.Aws;
using CloudServiceBus.Core.DataStores.Spec;

namespace CloudServiceBus.Core.DataStores
{
    public class SimpleDBDataStore : IDataStore
    {
        private AmazonSimpleDB _simpleDb;

        public SimpleDBDataStore()
        {
            _simpleDb = AwsFacade.GetSimpleDBClient();
        }

        public void Flush(string storeIdentifier)
        {
            var deleteDomainAction = new DeleteDomainRequest().WithDomainName(storeIdentifier);
            _simpleDb.DeleteDomain(deleteDomainAction);
        }

        public void Add(string storeIdentifier, string requestIdentifier, string[] responseItems)
        {
            EnsureDomain(storeIdentifier);

            foreach (var responseItem in responseItems)
            {
                var itemName = Guid.NewGuid().ToString();
                var putRequest = new PutAttributesRequest()
                                        .WithDomainName(storeIdentifier)
                                        .WithItemName(itemName);
                List<ReplaceableAttribute> attributes = putRequest.Attribute;
                attributes.Add(new ReplaceableAttribute()
                                    .WithName("RequestId")
                                    .WithValue(requestIdentifier));
                attributes.Add(new ReplaceableAttribute()
                                    .WithName("ResponseItem")
                                    .WithValue(responseItem));
                _simpleDb.PutAttributes(putRequest);
            }
        }

        private void EnsureDomain(string storeIdentifier)
        {
            var createDomainRequest = new CreateDomainRequest().WithDomainName(storeIdentifier);
            _simpleDb.CreateDomain(createDomainRequest);
        }

        public string[] Fetch(string storeIdentifier, string requestIdentifier)
        {
            EnsureDomain(storeIdentifier);

            var response = new List<string>();

            var select = string.Format("select * from `{0}` where RequestId = '{1}'", storeIdentifier, requestIdentifier);
            var selectRequest = new SelectRequest().WithSelectExpression(select);
            var selectResponse = _simpleDb.Select(selectRequest);
            Console.WriteLine("Fetching select: {0}", select);
            if (selectResponse.IsSetSelectResult())
            {
                var selectResult = selectResponse.SelectResult;
                foreach (Item item in selectResult.Item)
                {
                    Console.WriteLine("  Item");
                    if (item.IsSetName())
                    {
                        Console.WriteLine("    Name: {0}", item.Name);
                    }
                    foreach (var attribute in item.Attribute)
                    {
                        if (attribute.IsSetName() && attribute.Name == "ResponseItem")
                        {
                            response.Add(attribute.Value);
                            break;
                        }
                    }
                }
            }
            return response.ToArray();
        }

        public bool Exists(string storeIdentifier, string requestIdentifier)
        {
            EnsureDomain(storeIdentifier);

            var count = 0;
            var select = string.Format("select count(*) from `{0}` where RequestId = '{1}'", storeIdentifier, requestIdentifier);
            var selectRequest = new SelectRequest().WithSelectExpression(select);
            var selectResponse = _simpleDb.Select(selectRequest);
            Console.WriteLine("Fetching select: {0}", select);
            if (selectResponse.IsSetSelectResult())
            {
                var selectResult = selectResponse.SelectResult;
                foreach (Item item in selectResult.Item)
                {
                    foreach (var attribute in item.Attribute)
                    {
                        if (attribute.IsSetName() && attribute.Name == "Count")
                        {
                            int.TryParse(attribute.Value, out count);
                            break;
                        }
                    }
                }
            }

            return count > 0;
        }
    }
}
