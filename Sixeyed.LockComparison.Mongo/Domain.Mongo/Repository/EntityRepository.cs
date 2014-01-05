using Domain.Ef.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Domain.Mongo
{
    public class EntityRepository
    {
        public void Save(Entity entity, WriteConcern concern = null)
        {
            var collection = Db.GetCollection<Entity>();
            collection.Save(entity, concern ?? WriteConcern.Acknowledged);
        }

        public Entity Get(ObjectId id)
        {
            var collection = Db.GetCollection<Entity>();
            return collection.FindOneById(id);
        }

        public long Count()
        {
            var collection = Db.GetCollection<Entity>();
            return collection.Count();
        }

        public MongoCollection<Entity> GetCollection()
        {
            return Db.GetCollection<Entity>();
        }
    }
}
