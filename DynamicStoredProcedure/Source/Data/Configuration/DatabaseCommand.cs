using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace DynamicSP.Data.Configuration
{
    /// <summary>
    /// Section for configuring database commands
    /// </summary>
    public class DatabaseCommand : ConfigurationElement
    {
        /// <summary>
        /// Class name of command to configure
        /// </summary>
        [ConfigurationProperty(DatabaseCommand.SettingName.CommandTypeName)]
        public string CommandTypeName
        {
            get { return (string)this[SettingName.CommandTypeName]; }
        }

        /// <summary>
        /// Timeout before executing commands are terminated
        /// </summary>
        [ConfigurationProperty(DatabaseCommand.SettingName.CommandTimeout)]
        public int CommandTimeout
        {
            get { return (int)this[SettingName.CommandTimeout]; }
        }

        /// <summary>
        /// Constants for indexing settings
        /// </summary>
        private struct SettingName
        {
            public const string CommandTypeName = "commandTypeName";
            public const string CommandTimeout = "commandTimeout";
        }
    }
}
