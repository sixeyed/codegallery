using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TplSample.Service;

namespace TplSample
{
    public class ServiceWrapper
    {
        private static IEnumerable<string> GetItems(int collectionSize)
        {
            var collection = new List<string>(collectionSize);
            for (int i = 0; i < collectionSize; i++)
            {
                collection.Add(Guid.NewGuid().ToString());
            }
            return collection;
        }

        public void ProcessSequential(int collectionSize)
        {
            foreach (var item in GetItems(collectionSize))
            {
                ServiceCall(item);
            }
        }

        public void ProcessWithParallelForEach(int collectionSize)
        {
            Parallel.ForEach(GetItems(collectionSize), x => ServiceCall(x));
        }

        public void ProcessWithTasks(int collectionSize)
        {
            var collection = GetItems(collectionSize).ToArray();
            var tasks = new Task[collectionSize];
            for (int i = 0; i < collectionSize; i++)
            {
                var item = collection[i];
                tasks[i] = Task.Factory.StartNew(() => ServiceCall(item));
            }
            Task.WaitAll(tasks);
        }

        private static void ServiceCall(string id)
        {
            using (var client = new StorageServiceClient())
            {
                client.Store(id);
                client.Close();
            }
        }
    }
}
