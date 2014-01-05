using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Ef.Model;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Ef.Tests
{
    [TestClass]
    public class ReuseEntityContextWthNoLockTests
    {
        [TestInitialize]
        public void Setup()
        {
            using (var context = new EntityContext())
            {
                context.Database.ExecuteSqlCommand("DELETE EntityEvents");
                context.Database.ExecuteSqlCommand("DELETE Entities");
                context.Database.ExecuteSqlCommand("DELETE ParentEntities");
            }
        }

        [TestMethod]
        public void Sql_ReuseContext_WithNoLock_RunEntity_Single()
        {
            RunEntity();
            using (var context = new EntityContext())
            {
                Assert.AreEqual(1, context.ParentEntities.Count());
                Assert.AreEqual(1, context.Entities.Count());
            }
        }

        [TestMethod]
        public void Sql_ReuseContext_WithNoLock_RunEntity_1000_at_100()
        {
            RunBatch(100, 10);
        }

        [TestMethod]
        public void Sql_ReuseContext_WithNoLock_RunEntity_10000_at_100()
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
            using (var context = new EntityContext())
            {
                Assert.AreEqual(batchCount * batchSize, context.ParentEntities.Count());
                Assert.AreEqual(batchCount * batchSize, context.Entities.Count());
            }
        }

        private void RunEntity()
        {
            //var stopwatch = Stopwatch.StartNew();
            var correlationId = Guid.NewGuid().ToString();
            using (new NoLockScope())
            {
                using (var context = new EntityContext())
                {
                    var parent = context.ParentEntities.FirstOrDefault(x => x.CorrelationId == correlationId);
                    if (parent == null)
                    {
                        parent = new ParentEntity
                        {
                            CorrelationId = correlationId
                        };
                        context.ParentEntities.Add(parent);
                        context.SaveChanges();
                    }
                    var entity = new Entity
                    {
                        ParentId = parent.Id,
                        Name = Guid.NewGuid().ToString(),
                        StatusCode = 0
                    };
                    context.Entities.Add(entity);
                    context.SaveChanges();
                    for (int i = 0; i < 15; i++)
                    {
                        var statusCode = i;
                        var entityId = entity.Id;
                        AddEvent(entity.Id, statusCode, context);
                    }
                    //Console.WriteLine("* Entity id: {0}, took: {1}ms", entity.Id, stopwatch.ElapsedMilliseconds);
                }
            }
        }

        private void AddEvent(int id, int statusCode, EntityContext context)
        {
            var entity = context.Entities.Find(id);
            entity.StatusCode = statusCode;
            entity.Events.Add(new EntityEvent
            {
                CreatedAt = DateTime.Now,
                Description = Guid.NewGuid().ToString(),
                StatusCode = statusCode
            });
            context.SaveChanges();
        }
    }
}
