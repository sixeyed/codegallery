using System;
using System.Configuration;
using System.Xml;
using ConfigurableCrossReferenceCache.Logging;

namespace ConfigurableCrossReferenceCache.Configuration
{
    /// <summary>
    /// Element for configuring a location
    /// </summary>
    public class CacheElement : ConfigurationElement
    {
        /// <summary>
        /// Returns the Cache key
        /// </summary>
        [ConfigurationProperty(CacheElement.SettingName.Key)]
        public string Key
        {
            get { return (string)this[CacheElement.SettingName.Key]; }
        }

        /// <summary>
        /// Returns the Lifespan as a string in xsd:duration format
        /// </summary>
        [ConfigurationProperty(CacheElement.SettingName.Lifespan, DefaultValue = CacheElement.Default.LifespanDuration)]
        public string Lifespan
        {
            get { return (string)this[CacheElement.SettingName.Lifespan]; }
        }

        /// <summary>
        /// Returns the Lifespan as a Timespan
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetLifespan()
        {
            string lifespanDuration = Lifespan;
            TimeSpan lifespan;
            try
            {
                lifespan = XmlConvert.ToTimeSpan(lifespanDuration.Trim().ToUpper());
            }
            catch (FormatException ex)
            {
                lifespan = XmlConvert.ToTimeSpan(CacheElement.Default.LifespanDuration);
                Log.Warn("Cache lifespan value: {0} for key: {1} is not valid. Using default lifespan: {2}",
                            lifespanDuration, Key, CacheElement.Default.LifespanDuration);
            }
            return lifespan;
        }

        /// <summary>
        /// Constants for indexing settings
        /// </summary>
        private struct SettingName
        {
            /// <summary>
            /// key
            /// </summary>
            public const string Key = "key";

            /// <summary>
            /// lifespan
            /// </summary>
            public const string Lifespan = "lifespan";
        }

        private struct Default
        {
            public const string LifespanDuration = "PT30M";
        }
    }
}
