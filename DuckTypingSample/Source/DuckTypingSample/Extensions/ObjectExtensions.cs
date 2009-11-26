using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckTypingSample.Extensions
{
    /// <summary>
    /// Object extension methods
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns the object, duck-typed to the given interface
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TInterface As<TInterface>(this object obj)
        {
            return DuckType.As<TInterface>(obj);
        }
    }
}
