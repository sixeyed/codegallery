using System.Configuration;
using DistributedServiceSample.Core.Configuration.Elements;

namespace DistributedServiceSample.Core.Configuration.Collections
{
    [ConfigurationCollection(typeof (ServiceElement),
        AddItemName = "service",
        CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ServiceCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Returns the element name used for serializing the collection
        /// </summary>
        protected override string ElementName
        {
            get { return "service"; }
        }

        /// <summary>
        /// Returns the collection type used (BasicMap)
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        /// <summary>
        /// Gets the regular expression configured at the specified index
        /// </summary>
        /// <param name="index">Collection index of report to return</param>
        /// <returns><see cref="CacheTargetElement"/></returns>
        public ServiceElement this[int index]
        {
            get { return (ServiceElement)BaseGet(index); }
        }

        /// <summary>
        /// Gets the regular expression configured with the specified name
        /// </summary>
        /// <param name="key">Name of regular expression to return</param>
        /// <returns><see cref="CacheTargetElement"/></returns>
        public new ServiceElement this[string key]
        {
            get { return (ServiceElement)BaseGet(key); }
        }

        /// <summary>
        /// Returns a new collection element
        /// </summary>
        /// <returns>New <see cref="CacheTargetElement"/></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceElement();
        }

        /// <summary>
        /// Returns the collection key value
        /// </summary>
        /// <param name="element"><see cref="ServiceElement"/> element</param>
        /// <returns><see cref="ServiceElement"/> key value</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceElement)element).Contract;
        }
    }
}