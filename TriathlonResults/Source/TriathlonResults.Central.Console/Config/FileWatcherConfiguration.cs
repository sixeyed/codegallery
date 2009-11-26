using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace TriathlonResults.Central.Console.Config
{
    public class FileWatcherConfiguration : ConfigurationSection
    {
        private static FileWatcherConfiguration current;

        /// <summary>
        /// Sets up the current settings from application config
        /// </summary>
        static FileWatcherConfiguration()
        {
            current = ConfigurationManager.GetSection("fileWatcherConfiguration") as FileWatcherConfiguration;
        }

        /// <summary>
        /// Returns the currently configured settings
        /// </summary>
        public static FileWatcherConfiguration Current
        {
            get { return current; }
        }

        [ConfigurationProperty(FileWatcherConfiguration.SettingName.Path)]
        public string Path
        {
            get { return (string)this[SettingName.Path]; }
        }

        [ConfigurationProperty(FileWatcherConfiguration.SettingName.Filter)]
        public string Filter
        {
            get { return (string)this[SettingName.Filter]; }
        }

        [ConfigurationProperty(FileWatcherConfiguration.SettingName.Enabled)]
        public bool Enabled
        {
            get { return (bool)this[SettingName.Enabled]; }
        }

        [ConfigurationProperty(FileWatcherConfiguration.SettingName.FieldDelimiter)]
        public string FieldDelimiter
        {
            get { return (string)this[SettingName.FieldDelimiter]; }
        }

        private struct SettingName
        {
            public const string Path = "path";
            public const string Filter = "filter";
            public const string Enabled = "enabled";
            public const string FieldDelimiter = "fieldDelimiter";
        }
    }
}
