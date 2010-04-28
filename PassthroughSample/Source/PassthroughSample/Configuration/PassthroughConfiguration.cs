using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using PassthroughSample.Configuration.Collections;

namespace PassthroughSample.Configuration
{
    public class PassthroughConfiguration : ConfigurationSection
    {
        /// <summary>
        /// Returns the currently configured settings
        /// </summary>
        public static PassthroughConfiguration Current
        {
            get
            {
                return ConfigurationManager.GetSection("passthrough") as PassthroughConfiguration;
            }
        }

        [ConfigurationProperty(PassthroughConfiguration.SettingName.Types)]
        public TypeCollection Types
        {
            get { return (TypeCollection)this[SettingName.Types]; }
        }

        /// <summary>
        /// Constants for indexing settings
        /// </summary>
        private struct SettingName
        {
            public const string Types = "types";
        }
    }
}
