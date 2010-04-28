using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace PassthroughSample
{
    /// <summary>
    /// Extensions to <see cref="Type"/>
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns the full type name and assembly display name of the type
        /// </summary>
        /// <example>System.String, System</example>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string FullNameWithAssemblyName(this Type type)
        {
            return string.Format("{0}, {1}", type.FullName, type.Assembly.GetName().Name);
        }
    }
}
