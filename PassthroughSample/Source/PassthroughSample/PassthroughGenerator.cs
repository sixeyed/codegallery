using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using Castle.Core.Interceptor;
using System.Reflection;

namespace PassthroughSample
{
    /// <summary>
    /// Proxy generator for wrapping passthrough objects
    /// </summary>
    public static class PassthroughGenerator
    {
        private static Dictionary<string, bool> _implementsCache = new Dictionary<string, bool>();
        private static readonly IDictionary<Type, ProxyGenerator> _generatorCache = new Dictionary<Type, ProxyGenerator>();

        internal static T As<T>(PassthroughInterceptor passthrough)
        {
            T proxy = default(T);
            Type type = passthrough.Inner.GetType();
            proxy = GenerateProxy<T>(type, passthrough);
            return proxy;
        }

        private static TInterface GenerateProxy<TInterface>(Type type, PassthroughInterceptor passthrough)
        {
            ProxyGenerator generator = GetProxyGenerator(type);
            return generator.CreateInterfaceProxyWithoutTarget<TInterface>(passthrough);
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