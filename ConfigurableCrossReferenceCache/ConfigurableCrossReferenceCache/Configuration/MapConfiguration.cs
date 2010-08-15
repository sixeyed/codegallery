using System.Configuration;
using ConfigurableCrossReferenceCache.Logging;

namespace ConfigurableCrossReferenceCache.Configuration
{
    /// <summary>
    /// Configuration section for mapping helper classes
    /// </summary>
    public class MapConfiguration : ConfigurationSection
    {
        private static bool _loggedWarning;

        /// <summary>
        /// Returns the currently configured settings
        /// </summary>
        public static MapConfiguration Current
        {
            get
            {
                MapConfiguration current = ConfigurationManager.GetSection(SectionName.Maps) as MapConfiguration;
                if (current == null)
                {
                    current = new MapConfiguration();
                    if (!_loggedWarning)
                    {
                        Log.Warn("Configuration section: {0} not specified. Default configuration will be used", SectionName.Maps);
                        _loggedWarning = true;
                    }
                }
                return current;
            }
        }

        /// <summary>
        /// Returns the collection of offices to ignore in building room descriptions
        /// </summary>
        [ConfigurationProperty(MapConfiguration.SettingName.Caching)]
        public CacheCollection Caching
        {
            get { return (CacheCollection)this[SettingName.Caching]; }
        }

        /// <summary>
        /// Constants for indexing settings
        /// </summary>
        private struct SettingName
        {
            /// <summary>
            /// caching
            /// </summary>
            public const string Caching = "caching";
        }

        private struct SectionName
        {
            public const string Maps = "crossReference";
        }
    }
}
