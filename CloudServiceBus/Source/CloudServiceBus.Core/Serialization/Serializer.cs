
namespace CloudServiceBus.Core.Serialization
{
    /// <summary>
    /// Represents a simple object serializer
    /// </summary>
    public static class Serializer
    {
        static Serializer()
        {
            Container.Register<ISerializer, JsonSerializer>(Lifetime.Transient);
        }

        public static ISerializer Current
        {
            get { return Container.Get<ISerializer>(); }
        }
    }
}
