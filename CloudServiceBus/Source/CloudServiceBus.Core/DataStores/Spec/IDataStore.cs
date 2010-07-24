
namespace CloudServiceBus.Core.DataStores.Spec
{
    public interface IDataStore
    {
        void Flush(string storeIdentifier);

        void Add(string storeIdentifier, string requestIdentifier, string[] responseItems);

        string[] Fetch(string storeIdentifier, string requestIdentifier);

        bool Exists(string storeIdentifier, string requestIdentifier);
    }
}
