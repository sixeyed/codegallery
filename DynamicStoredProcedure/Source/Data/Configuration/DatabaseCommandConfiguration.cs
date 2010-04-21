using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using DynamicSP.Core.Logging;

namespace DynamicSP.Data.Configuration
{
    /// <summary>
    /// Datbase section for configuring multiple DatabaseCommands
    /// </summary>
    public class DatabaseCommandConfiguration : ConfigurationSection
    {
        private static DatabaseCommandConfiguration _current;

        /// <summary>
        /// Sets up the current settings from application config
        /// </summary>
        static DatabaseCommandConfiguration()
        {
            _current = ConfigurationManager.GetSection("databaseCommandConfiguration") as DatabaseCommandConfiguration;
            if (_current == null)
            {
                Log.Warn("Configuration section: databaseCommandConfiguration not specified. Default configuration will be used");
                _current = new DatabaseCommandConfiguration();
            }
        }

        /// <summary>
        /// Returns the currently configured settings
        /// </summary>
        public static DatabaseCommandConfiguration Current
        {
            get { return _current; }
        }

        /// <summary>
        /// Returns the collection of configured DatabaseCommands
        /// </summary>
        [ConfigurationProperty(DatabaseCommandConfiguration.SettingName.EnablePerformanceHitCounters, DefaultValue = false)]
        public bool EnablePerformanceHitCounters
        {
            get { return (bool)this[SettingName.EnablePerformanceHitCounters]; }
        }

        /// <summary>
        /// Returns the collection of configured DatabaseCommands
        /// </summary>
        [ConfigurationProperty(DatabaseCommandConfiguration.SettingName.EnablePerformanceDurationCounters, DefaultValue = false)]
        public bool EnablePerformanceDurationCounters
        {
            get { return (bool)this[SettingName.EnablePerformanceDurationCounters]; }
        }

        /// <summary>
        /// Returns the collection of configured DatabaseCommands
        /// </summary>
        [ConfigurationProperty(DatabaseCommandConfiguration.SettingName.Commands)]
        public DatabaseCommandCollection Commands
        {
            get { return (DatabaseCommandCollection)this[SettingName.Commands]; }
        }

        /// <summary>
        /// Constants for indexing settings
        /// </summary>
        private struct SettingName
        {
            public const string EnablePerformanceHitCounters = "enablePerformanceHitCounters";
            public const string EnablePerformanceDurationCounters = "enablePerformanceDurationCounters";
            public const string Commands = "commands";
        }
    }
}
