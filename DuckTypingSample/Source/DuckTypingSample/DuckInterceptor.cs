using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Castle.Core.Interceptor;

namespace DuckTypingSample
{
    /// <summary>
    /// Interceptor calling an underlying method on a real object
    /// </summary>
    internal class DuckInterceptor : IInterceptor
    {
        private object _realObject;

        /// <summary>
        /// Constructor with real object
        /// </summary>
        /// <param name="realObject"></param>
        public DuckInterceptor(object realObject)
        {
            _realObject = realObject;
        }

        /// <summary>
        /// Intercepts the invocation and invokes it on the real object
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            MethodInfo realMethod = _realObject.GetType().GetMethod(invocation.Method.Name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            invocation.ReturnValue = realMethod.Invoke(_realObject, invocation.Arguments);
        }
    }
}
