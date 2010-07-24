using CloudServiceBus.Core.DataStores.Spec;

namespace CloudServiceBus.Core.DataStores
{
    public static class DataStore
    {
        static DataStore()
        {
            Container.Register<IDataStore, SimpleDBDataStore>(Lifetime.Transient);
        }

        public static IDataStore Current
        {
            get { return Container.Get<IDataStore>(); }
        }
    }
}
