using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace TriathlonResults.Console
{
    /// <summary>
    /// Configuration settings for sector result console app
    /// </summary>
    public class OutputFileConfiguration : ConfigurationSection
    {
        private static OutputFileConfiguration current;

        /// <summary>
        /// Sets up the current settings from application config
        /// </summary>
        static OutputFileConfiguration()
        {
            current = ConfigurationManager.GetSection("outputFileConfiguration") as OutputFileConfiguration;
        }

        /// <summary>
        /// Returns the currently configured settings
        /// </summary>
        public static OutputFileConfiguration Current
        {
            get { return current; }
        }

        [ConfigurationProperty(OutputFileConfiguration.SettingName.Path)]
        public string Path
        {
            get { return (string)this[SettingName.Path]; }
        }

        [ConfigurationProperty(OutputFileConfiguration.SettingName.FileNameFormat)]
        public string FileNameFormat
        {
            get { return (string)this[SettingName.FileNameFormat]; }
        }

        [ConfigurationProperty(OutputFileConfiguration.SettingName.FieldDelimiter)]
        public string FieldDelimiter
        {
            get { return (string)this[SettingName.FieldDelimiter]; }
        }

        private struct SettingName
        {
            public const string Path = "path";
            public const string FileNameFormat = "fileNameFormat";
            public const string FieldDelimiter = "fieldDelimiter";
        }
    }
}
