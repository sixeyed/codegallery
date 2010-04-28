using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using PassthroughSample.Configuration;
using PassthroughSample.Configuration.Elements;

namespace PassthroughSample
{
    /// <summary>
    /// Passthrough class, wrapper for setting up passthrough objects
    /// </summary>
    public class Passthrough
    {
        /// <summary>
        /// Interceptor which passes through calls on an outer object to calls
        /// on an inner object
        /// </summary>
        internal PassthroughInterceptor _interceptor = null;

        /// <summary>
        /// Starts setting up a passthrough object
        /// </summary>
        /// <param name="innerTypeName">Full name of the inner object type</param>
        /// <returns></returns>
        public Passthrough Create(string innerTypeName)
        {
            Type innerType = Type.GetType(innerTypeName);
            return Create(innerType);
        }

        /// <summary>
        /// Starts setting up a passthrough object
        /// </summary>
        /// <param name="innerType">Inner object type</param>
        /// <returns></returns>
        public Passthrough Create(Type innerType)
        {
            object inner = Activator.CreateInstance(innerType);
            _interceptor = new PassthroughInterceptor(inner);
            return this;
        }

        public Passthrough Create(object inner)
        {
            _interceptor = new PassthroughInterceptor(inner);
            return this;
        }

        /// <summary>
        /// Adds a member passthrough to the setup
        /// </summary>
        /// <param name="outerMemberName">Name of the outer member</param>
        /// <param name="innerMemberName">Name of the inner member</param>
        /// <returns></returns>
        public Passthrough WithPassthrough(string outerMemberName, string innerMemberName)
        {
            if (_interceptor != null)
            {
                _interceptor.Passthrough(outerMemberName, innerMemberName);
            }
            return this;
        }        

        /// <summary>
        /// Returns the setup passthrough as an instance of the specified outer type
        /// </summary>
        /// <typeparam name="T">Outer type</typeparam>
        /// <returns></returns>
        public T As<T>()
        {
            return PassthroughGenerator.As<T>(_interceptor);
        }

        /// <summary>
        /// Returns a passthrough configured using <see cref="PassthroughConfiguration"/>
        /// </summary>
        /// <typeparam name="T">Outer type</typeparam>
        /// <returns></returns>
        public static T GetConfigured<T>()
        {
            T passthrough = default(T);
            string outerTypeName = typeof(T).FullNameWithAssemblyName();
            var type = PassthroughConfiguration.Current.Types[outerTypeName];
            if (type != null)
            {
                var setup = new Passthrough();
                setup.Create(type.PassesThroughTo);
                foreach (MemberElement member in type.Members)
                {
                    setup.WithPassthrough(member.Name, member.PassesThroughTo);
                }
                passthrough = setup.As<T>();
            }
            return passthrough;
        }

        /// <summary>
        /// Returns a passthrough configured using <see cref="PassthroughConfiguration"/>
        /// </summary>
        /// <typeparam name="T">Outer type</typeparam>
        /// <param name="inner">Inner implementation</param>
        /// <returns></returns>
        public static T GetConfigured<T>(object inner)
        {
            T passthrough = default(T);
            string outerTypeName = typeof(T).FullNameWithAssemblyName();
            var type = PassthroughConfiguration.Current.Types[outerTypeName];
            if (type != null)
            {
                var setup = new Passthrough();
                setup.Create(inner);
                foreach (MemberElement member in type.Members)
                {
                    setup.WithPassthrough(member.Name, member.PassesThroughTo);
                }
                passthrough = setup.As<T>();
            }
            return passthrough;
        }
    }

    /// <summary>
    /// Generic version of <see cref="Passthrough"/>, for setting up passthrough
    /// objects using typed generic calls and lambda expressions
    /// </summary>
    /// <typeparam name="TOuter">Outer object type</typeparam>
    /// <typeparam name="TInner">Inner object type</typeparam>
    public class Passthrough<TOuter, TInner> : Passthrough
        where TInner : class, new()
    {
        /// <summary>
        /// Starts setting up a passthrough object
        /// </summary>
        /// <returns></returns>
        public Passthrough<TOuter, TInner> Create()
        {
            object inner = new TInner();
            _interceptor = new PassthroughInterceptor(inner);
            return this;
        }

        /// <summary>
        /// Starts setting up a passthrough object
        /// </summary>
        /// <param name="inner">Inner object instance</param>
        /// <returns></returns>
        public Passthrough<TOuter, TInner> Create(TInner inner)
        {
            _interceptor = new PassthroughInterceptor(inner);
            return this;
        }

        /// <summary>
        /// Adds a member passthrough to the setup
        /// </summary>
        /// <param name="outerMemberAccessor">Accessor expression for the outer member</param>
        /// <param name="innerMemberAccessor">Accessor expression for the inner member</param>
        /// <returns></returns>
        public Passthrough<TOuter, TInner> WithPassthrough(Expression<Func<TOuter, object>> outerMemberAccessor, Expression<Func<TInner, object>> innerMemberAccessor)
        {
            WithPassthrough((LambdaExpression)outerMemberAccessor, (LambdaExpression)innerMemberAccessor);
             return this;
        }

        /// <summary>
        /// Adds a member passthrough to the setup
        /// </summary>
        /// <param name="outerMemberAccessor">Accessor expression for the outer member</param>
        /// <param name="innerMemberAccessor">Accessor expression for the inner member</param>
        /// <returns></returns>
        public Passthrough<TOuter, TInner> WithPassthrough(LambdaExpression outerMemberAccessor, LambdaExpression innerMemberAccessor)
        {
            var outerMemberName = outerMemberAccessor.AsMemberName();
            var innerMemberName = innerMemberAccessor.AsMemberName();
            if (!string.IsNullOrEmpty(outerMemberName) && !string.IsNullOrEmpty(innerMemberName))
            {
                WithPassthrough(outerMemberName, innerMemberName);
            }
            return this;
        }

        /// <summary>
        /// Returns the setup passthrough as an instance of the outer type
        /// </summary>
        /// <returns></returns>
        public TOuter As()
        {
            return As<TOuter>();
        }
    }
}
