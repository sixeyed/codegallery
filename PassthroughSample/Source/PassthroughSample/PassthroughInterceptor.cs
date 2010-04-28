using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Castle.Core.Interceptor;

namespace PassthroughSample
{
    /// <summary>
    /// Interceptor calling an underlying method on a real object
    /// </summary>
    internal class PassthroughInterceptor : IInterceptor
    {
        internal object Inner { get; set; }
        private Type _innerType;

        /// <summary>
        /// Constructor with real object
        /// </summary>
        /// <param name="inner"></param>
        public PassthroughInterceptor(object inner)
        {
            Inner = inner;
            _innerType = inner.GetType();
        }

        private static BindingFlags InstanceBindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private Dictionary<string, string> _passthroughs = new Dictionary<string, string>();

        public void Passthrough(string outerMemberName, string innerMemberName)
        {
            _passthroughs[outerMemberName] = innerMemberName;
        }

        /// <summary>
        /// Intercepts the invocation and invokes it on the real object
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name.StartsWith("get_"))
            {
                invocation.ReturnValue = GetPropertyValue(invocation.Method.Name.Substring(4));
            }
            else if (invocation.Method.Name.StartsWith("set_"))
            {
                SetPropertyValue(invocation.Method.Name.Substring(4), invocation.Arguments[0]);
            }
            else
            {
                invocation.ReturnValue = InvokeMethod(invocation.Method.Name, invocation.Arguments);
            }
        }

        private object InvokeMethod(string name, object[] arguments)
        {
            object result = null;
            string targetName = GetInnerMemberName(name);
            var method = (from info in Inner.GetType().GetMethods(InstanceBindings)
                            where info.Name == targetName
                            select info).FirstOrDefault();

            if (method != null)
            {
               result =  method.Invoke(Inner, arguments);
            }
            return result;
        }

        private void SetPropertyValue(string name, object value)
        {
            string targetName = GetInnerMemberName(name);
            var property = (from info in Inner.GetType().GetProperties(InstanceBindings)
                            where info.Name == targetName
                            && info.PropertyType == value.GetType()
                            && info.CanWrite
                            select info).FirstOrDefault();

            if (property != null)
            {
                property.SetValue(Inner, value, null);
            }
        }

        private string GetInnerMemberName(string outerMemberName)
        {
            string targetName = _passthroughs[outerMemberName];
            if (string.IsNullOrEmpty(targetName))
            {
                targetName = outerMemberName;
            }
            return targetName;
        }


        private object GetPropertyValue(string name)
        {
            object result = null;

            string targetName = GetInnerMemberName(name);

            var property = (from info in Inner.GetType().GetProperties(InstanceBindings)
                            where info.Name == targetName
                            && info.CanRead
                            select info).FirstOrDefault();

            if (property != null)
            {
                result = property.GetValue(Inner, null);
            }

            return result;
        }
    }
}
