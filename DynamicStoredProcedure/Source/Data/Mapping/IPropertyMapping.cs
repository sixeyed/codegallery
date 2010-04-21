using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections;

namespace DynamicSP.Data.Mapping
{
    /// <summary>
    /// Interface for mapping a property value
    /// </summary>
    public interface IPropertyMapping
    {
        /// <summary>
        /// Gets/sets the column name for the source data
        /// </summary>
        string SourceColumnName { get; set; }

        /// <summary>
        /// Gets the property to be populated
        /// </summary>
        PropertyInfo PropertyInfo { get; set; }

        /// <summary>
        /// Returns whether the mapping include conversion
        /// </summary>
        bool HasConversion { get; }

        /// <summary>
        /// Coverts input value to output type
        /// </summary>
        /// <param name="input">Input value</param>
        /// <returns>Converted output</returns>
        object Convert(object input);
    }
}