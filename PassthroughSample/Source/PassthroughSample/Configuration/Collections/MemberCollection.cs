using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Collections.ObjectModel;
using PassthroughSample.Configuration.Elements;

namespace PassthroughSample.Configuration.Collections
{
    /// <summary>
    /// Collection of passthrough member settings
    /// </summary>
    [ConfigurationCollection(typeof(MemberElement), 
        AddItemName="member", 
        CollectionType=ConfigurationElementCollectionType.BasicMap)]
    public class MemberCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MemberElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MemberElement)element).Name;
        }

        protected override string ElementName
        {
            get { return "member"; }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }
    }
}
