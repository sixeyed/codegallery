using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections;

namespace FluentDAL.Mapping
{
    /// <summary>
    /// Interface for mapping between entities and IDataReaders
    /// </summary>
    public interface IDataReaderMap
    {
        /// <summary>
        /// Gets the target entity type for mapping
        /// </summary>
        Type TargetType { get; }

        /// <summary>
        /// Populates the supplied entity from the current row of the data reader
        /// </summary>
        /// <param name="entity">Entity to populate</param>
        /// <param name="reader">Open data reader, positioned at start point for population</param>
        void Populate(object entity, IDataReader reader);

        /// <summary>
        /// Creates and populates an entity from the contents of a data reader
        /// </summary>
        /// <param name="reader">Open data reader, positioned at start point for population</param>
        /// <returns>Populated entity</returns>
        object Create(IDataReader reader);

        /// <summary>
        /// Populates a list of entities from the contents of a data reader
        /// </summary>
        /// <param name="list">List to populate</param>
        /// <param name="sourceReader">Open data reader</param>
        void PopulateList(IList list, IDataReader sourceReader);
    }
}
