using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Collections.ObjectModel;

namespace DynamicSP.Data.Configuration
{
    /// <summary>
    /// Collection of configured DatabaseCommand settings
    /// </summary>
    [ConfigurationCollection(typeof(DatabaseCommand), 
        AddItemName="command", 
        CollectionType=ConfigurationElementCollectionType.BasicMap)]
    public class DatabaseCommandCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Returns a new collection element
        /// </summary>
        /// <returns>New DatabaseCommand</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new DatabaseCommand();
        }

        /// <summary>
        /// Returns the collection key value
        /// </summary>
        /// <param name="element">DatabaseCommand element</param>
        /// <returns>DatabaseCommand key value</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DatabaseCommand)element).CommandTypeName;
        }

        /// <summary>
        /// Returns the element name used for serializing the collection
        /// </summary>
        protected override string ElementName
        {
            get { return "command"; }
        }

        /// <summary>
        /// Returns the collection type used (BasicMap)
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        /// <summary>
        /// Gets the report configured at the specified index
        /// </summary>
        /// <param name="index">Collection index of report to return</param>
        /// <returns>DatabaseCommand</returns>
        public DatabaseCommand this[int index]
        {
            get { return (DatabaseCommand)base.BaseGet(index); }
        }

        /// <summary>
        /// Gets the report configured with the specified name
        /// </summary>
        /// <param name="commandTypeName">Name of command type to return</param>
        /// <returns>DatabaseCommand</returns>
        public new DatabaseCommand this[string commandTypeName]
        {
            get { return (DatabaseCommand)base.BaseGet(commandTypeName); }
        }

        /// <summary>
        /// Gets all configured DatabaseCommands
        /// </summary>
        public ReadOnlyCollection<DatabaseCommand> GetCommands()
        {
            List<DatabaseCommand> commands = new List<DatabaseCommand>();
            object[] keys = this.BaseGetAllKeys();
            foreach (object key in keys)
            {
                DatabaseCommand command = this.BaseGet(key) as DatabaseCommand;
                commands.Add(command);
            }
            return new ReadOnlyCollection<DatabaseCommand>(commands);
        }
    }
}
