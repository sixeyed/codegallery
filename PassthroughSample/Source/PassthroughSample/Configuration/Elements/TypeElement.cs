using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using PassthroughSample.Configuration.Collections;

namespace PassthroughSample.Configuration.Elements
{
    /// <summary>
    /// Element for configuring passthrough types
    /// </summary>
    public class TypeElement : ConfigurationElement
    {
        [ConfigurationProperty(TypeElement.SettingName.Name)]
        public string Name
        {
            get { return (string)this[SettingName.Name]; }
        }

        [ConfigurationProperty(TypeElement.SettingName.PassesThroughTo)]
        public string PassesThroughTo
        {
            get { return (string)this[SettingName.PassesThroughTo]; }
        }

        [ConfigurationProperty(TypeElement.SettingName.Members)]
        public MemberCollection Members
        {
            get { return (MemberCollection)this[SettingName.Members]; }
        }

        /// <summary>
        /// Constants for indexing settings
        /// </summary>
        private struct SettingName
        {
            public const string Name = "name";
            public const string PassesThroughTo = "passesThroughTo";
            public const string Members = "members";
        }
    }
}
