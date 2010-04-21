using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections;
using DynamicSP.Core.Logging;

namespace DynamicSP.Data.Mapping
{
    /// <summary>
    /// Represents a property mapping without conversion
    /// </summary>
    public class PropertyMapping : IPropertyMapping
    {
        /// <summary>
        /// Gets/sets the column name for the source data
        /// </summary>
        public string SourceColumnName { get; set; }
        
        /// <summary>
        /// Gets the property to be populated
        /// </summary>
        public PropertyInfo PropertyInfo { get; set; }

        /// <summary>
        /// Returns whether the mapping include conversion
        /// </summary>
        public virtual bool HasConversion
        {
            get { return false; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PropertyMapping() { }

        /// <summary>
        /// Constructor with known state
        /// </summary>
        /// <param name="sourceColumnName">Name of source database column</param>
        /// <param name="info">Target property to map</param>
        public PropertyMapping(string sourceColumnName, PropertyInfo info)
        {
            SourceColumnName = sourceColumnName;
            PropertyInfo = info;
        }

        /// <summary>
        /// Coverts input value to output type
        /// </summary>
        /// <param name="input">Input value</param>
        /// <returns>Converted output</returns>
        public virtual object Convert(object input)
        {
            return input;
        }
    }

    /// <summary>
    /// Represents a property mapping with conversion between types
    /// </summary>
    /// <typeparam name="TInput">Input data type</typeparam>
    /// <typeparam name="TOutput">Output data type</typeparam>
    public class PropertyMapping<TInput, TOutput> : PropertyMapping
    {
        /// <summary>
        /// Function to convert from input to output
        /// </summary>
        public Func<TInput, TOutput> Conversion { get; set; }

        /// <summary>
        /// Returns whether the mapping include conversion
        /// </summary>
        public override bool HasConversion
        {
            get { return (Conversion != null); }
        }

        /// <summary>
        /// Default condtructor
        /// </summary>
        public PropertyMapping() { }

        /// <summary>
        /// Constructor with initial state
        /// </summary>
        /// <param name="sourceColumnName">Name of source database column</param>
        /// <param name="info">Property to populate</param>
        /// <param name="conversion">Function to convert from input to output</param>
        public PropertyMapping(string sourceColumnName, PropertyInfo info, Func<TInput, TOutput> conversion)
        {
            SourceColumnName = sourceColumnName;
            PropertyInfo = info;
            Conversion = conversion;
        }

        /// <summary>
        /// Coverts input value to output type
        /// </summary>
        /// <param name="input">Input value</param>
        /// <returns>Converted output</returns>
        public override object Convert(object input)
        {
            object output = null;
            if (Conversion != null)
            {
                output = (object)Conversion.Invoke((TInput)input);
            }
            return output;
        }
    }
}