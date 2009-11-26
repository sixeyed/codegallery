using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using Castle.Core.Interceptor;
using System.Reflection;

namespace DuckTypingSample
{
    /// <summary>
    /// Duck-typing implementation
    /// </summary>
    public static class DuckType
    {
        private static Dictionary<string, bool> _implementsCache = new Dictionary<string, bool>();
        private static readonly IDictionary<Type, ProxyGenerator> _generatorCache = new Dictionary<Type, ProxyGenerator>();

        /// <summary>
        /// Returns the object, duck-typed to the given interface
        /// </summary>
        /// <remarks>
        /// Returns default value for TInterface if the given object
        /// does not tacitly implement the interface
        /// </remarks>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TInterface As<TInterface>(object obj)
        {
            TInterface duck = default(TInterface);
            Type type = obj.GetType();
            if (Implements<TInterface>(type))
            {
                duck = GenerateProxy<TInterface>(type, obj);
            }
            return duck;
        }

        private static bool Implements<TInterface>(Type type)
        {
            string key = string.Format("{0}:{1}", typeof(TInterface).FullName, type.FullName);
            if (!_implementsCache.ContainsKey(key))
            {
                int matchCount = 0;
                MemberInfo[] interfaceMembers = typeof(TInterface).GetMembers();
                foreach (MemberInfo member in interfaceMembers)
                {
                    if (type.GetMember(member.Name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length == 1)
                    {
                        matchCount++;
                    }
                    else
                    {
                        break;
                    }
                }
                _implementsCache.Add(key, (matchCount == interfaceMembers.Length));
            }
            return _implementsCache[key];
        }        

        private static TInterface GenerateProxy<TInterface>(Type type, object obj)
        {
            ProxyGenerator generator = GetProxyGenerator(type);
            return generator.CreateInterfaceProxyWithoutTarget<TInterface>(new DuckInterceptor(obj));
        }

        private static ProxyGenerator GetProxyGenerator(Type type)
        {
            if (!_generatorCache.ContainsKey(type))
            {
                _generatorCache[type] = new ProxyGenerator();
            }
            return _generatorCache[type];
        }
    }
}