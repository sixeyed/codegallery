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
    /// Interface for mapping a property value
    /// </summary>
    public interface IPropertyMapping
    {
        PropertyInfo PropertyInfo { get; set; }
        bool HasConversion { get; }
        object Convert(object input);
    }
}