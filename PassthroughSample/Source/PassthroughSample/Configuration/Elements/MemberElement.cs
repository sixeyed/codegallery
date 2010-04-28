using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace PassthroughSample.Configuration.Elements
{
    /// <summary>
    /// Element for configuring passthrough type members
    /// </summary>
    public class MemberElement : ConfigurationElement
    {
        [ConfigurationProperty(MemberElement.SettingName.Name)]
        public string Name
        {
            get { return (string)this[SettingName.Name]; }
        }

        [ConfigurationProperty(MemberElement.SettingName.PassesThroughTo)]
        public string PassesThroughTo
        {
            get { return (string)this[SettingName.PassesThroughTo]; }
        }

        /// <summary>
        /// Constants for indexing settings
        /// </summary>
        private struct SettingName
        {
            public const string Name = "name";
            public const string PassesThroughTo = "passesThroughTo";
        }
    }
}
