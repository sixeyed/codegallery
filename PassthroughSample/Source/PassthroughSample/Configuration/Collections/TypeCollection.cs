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
    /// Collection of passthrough type settings
    /// </summary>
    [ConfigurationCollection(typeof(TypeElement), 
        AddItemName="type", 
        CollectionType=ConfigurationElementCollectionType.BasicMap)]
    public class TypeCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TypeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TypeElement)element).Name;
        }

        protected override string ElementName
        {
            get { return "type"; }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        public new TypeElement this[string key]
        {
            get { return (TypeElement)base.BaseGet(key); }
        }
    }
}
