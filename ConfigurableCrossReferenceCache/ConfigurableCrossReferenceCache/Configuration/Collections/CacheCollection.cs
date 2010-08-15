using System;
using System.Configuration;

namespace ConfigurableCrossReferenceCache.Configuration
{
    /// <summary>
    /// Collection of configured <see cref="CacheElement"/>s
    /// </summary>
    [ConfigurationCollection(typeof(CacheElement),
        AddItemName = "cache", 
        CollectionType=ConfigurationElementCollectionType.BasicMap)]
    public class CacheCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Returns a new collection element
        /// </summary>
        /// <returns>New <see cref="CacheElement"/></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new CacheElement();
        }

        /// <summary>
        /// Returns the collection key value
        /// </summary>
        /// <param name="element"><see cref="CacheElement"/> element</param>
        /// <returns><see cref="CacheElement"/> key value</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CacheElement)element).Key;
        }

        /// <summary>
        /// Returns the element name used for serializing the collection
        /// </summary>
        protected override string ElementName
        {
            get { return "cache"; }
        }

        /// <summary>
        /// Returns the collection type used (BasicMap)
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        /// <summary>
        /// Returns the configured xsd:duration lifespan for the given cache key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetLifespanDuration(string key)
        {
            CacheElement element = base.BaseGet(key) as CacheElement;
            if (element == null)
            {
                element = new CacheElement();
            }
            return element.Lifespan;
        }

        /// <summary>
        /// Returns the configured lifespan for the given cache key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TimeSpan GetLifespan(string key)
        {
            CacheElement element = base.BaseGet(key) as CacheElement;
            if (element == null)
            {
                element = new CacheElement();
            }
            return element.GetLifespan();
        }
    }
}
