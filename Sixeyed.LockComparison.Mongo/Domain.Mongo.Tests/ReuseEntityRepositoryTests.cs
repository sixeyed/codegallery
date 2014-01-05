using Domain.Ef.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Domain.Mongo.Tests
{
    [TestClass]
    public class ReuseEntityRepositoryTests
    {
        [TestInitialize]
        public void Setup()
        {
            Db.GetCollection<ParentEntity>().Drop();
            Db.GetCollection<Entity>().Drop();
            Db.EnsureIndexes();
        }

        [TestMethod]
        public void Mongo_ReuseCollection_RunEntity_Single()
        {
            RunEntity();
            Assert.AreEqual(1, new ParentEntityRepository().Count());
            Assert.AreEqual(1, new EntityRepository().Count());
        }

        [TestMethod]
        public void Mongo_ReuseCollection_RunEntity_1000_at_100()
        {
            RunBatch(100, 10);
        }

        [TestMethod]
        public void Mongo_ReuseCollection_RunEntity_10000_at_100()
        {
            RunBatch(100, 100);
        }

        private void RunBatch(int batchSize, int batchCount)
        {
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < batchCount; i++)
            {
                var batchStopwatch = Stopwatch.StartNew();
                var tasks = new List<Task>();
                for (int j = 0; j < batchSize; j++)
                {
                    tasks.Add(new Task(() => RunEntity()));
                }
                tasks.ForEach(x => x.Start());
                Task.WaitAll(tasks.ToArray());
                Console.WriteLine("** Batch: {0}, took: {1}ms", i, batchStopwatch.ElapsedMilliseconds);
            }
            Console.WriteLine("*** Run took: {0}ms", stopwatch.ElapsedMilliseconds);
            Assert.AreEqual(batchCount * batchSize, new ParentEntityRepository().Count());
            Assert.AreEqual(batchCount * batchSize, new EntityRepository().Count());
        }

        private void RunEntity()
        {
            //var stopwatch = Stopwatch.StartNew();
            var correlationId = Guid.NewGuid().ToString();
            var parentRepository = new ParentEntityRepository();
            if (!parentRepository.Exists(correlationId))
            {
                var parent = new ParentEntity
                {
                    CorrelationId = correlationId
                };
                parentRepository.Save(parent);
            }
            var entity = new Entity
            {
                CorrelationId = correlationId,
                Name = Guid.NewGuid().ToString(),
                StatusCode = 0
            };
            var entityRepository = new EntityRepository();
            entityRepository.Save(entity);
            var collection = entityRepository.GetCollection();
            for (int i = 0; i < 15; i++)
            {
                var statusCode = i;
                var entityId = entity.Id;
                AddEvent(entityId, statusCode, collection);
            }
            //Console.WriteLine("* Entity id: {0}, took: {1}ms", entity.Id, stopwatch.ElapsedMilliseconds);
        }

        private void AddEvent(ObjectId id, int statusCode, MongoCollection<Entity> collection)
        {
            var entity = collection.FindOneById(id);
            entity.StatusCode = statusCode;
            entity.Events.Add(new EntityEvent
            {
                CreatedAt = DateTime.Now,
                Description = Guid.NewGuid().ToString(),
                StatusCode = statusCode
            });
            collection.Save(entity);
        }
    }
}
