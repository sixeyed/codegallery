using Domain.Ef.Model;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Domain.Mongo
{
    public class ParentEntityRepository
    {
        public bool Exists(string correlationId)
        {
            var queryBuilder = new QueryBuilder<ParentEntity>();
            var query = queryBuilder.Where(x=>x.CorrelationId == correlationId);
            var collection = Db.GetCollection<ParentEntity>();
            var matches = collection.Count(query);
            return matches > 0;
        }

        public void Save(ParentEntity parent, WriteConcern concern = null)
        {
            var collection = Db.GetCollection<ParentEntity>();
            collection.Save(parent, concern ?? WriteConcern.Acknowledged);
        }

        public long Count()
        {
            var collection = Db.GetCollection<ParentEntity>();
            return collection.Count();
        }
    }
}
