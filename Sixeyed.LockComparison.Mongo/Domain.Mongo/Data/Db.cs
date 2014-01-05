using Domain.Ef.Model;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mongo
{
    public static class Db
    {
        public static void EnsureIndexes()
        {
            var collection = GetCollection<ParentEntity>();
            collection.EnsureIndex(new IndexKeysBuilder().Descending("CorrelationId"));
        }

        private static MongoDatabase GetDatabase()
        {
            var connectionString = ConfigurationManager.AppSettings["mongodb"];
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            return server.GetDatabase("entities");
        }

        public static MongoCollection<T> GetCollection<T>()
        {
            var name = typeof(T).Name.Substring(0, 1).ToLower() + typeof(T).Name.Substring(1);
            var db = GetDatabase();
            return db.GetCollection<T>(name);
        }
    }
}
