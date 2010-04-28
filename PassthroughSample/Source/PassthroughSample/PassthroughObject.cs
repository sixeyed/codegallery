using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Reflection;

namespace DynamicPassthrough
{

    public interface ICache
    {
        string Name { get; set;  }
    }

    public class PassthroughObject : DynamicObject
    {
        public object _inner;

        public PassthroughObject(object inner)
        {
            _inner = inner;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            return base.TryInvokeMember(binder, args, out result);
        }

        private static BindingFlags AllBindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        private Dictionary<string, string> _passthroughs = new Dictionary<string, string>();

        public void AddPassthrough(string outerMemberName, string innerMemberName)
        {
            _passthroughs[outerMemberName] = innerMemberName;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string targetName = _passthroughs[binder.Name];
            if (string.IsNullOrEmpty(targetName))
            {
                targetName = binder.Name;
            }
            result = null;
            var property = (from info in _inner.GetType().GetProperties(AllBindings)
                            where info.Name == targetName
                           && info.CanRead
                           select info).FirstOrDefault();

            if (property != null)
            {
                result = property.GetValue(_inner, null);
                return true;
            }
            
            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            string targetName = _passthroughs[binder.Name];
            if (string.IsNullOrEmpty(targetName))
            {
                targetName = binder.Name;
            }
            var property = (from info in _inner.GetType().GetProperties(AllBindings)
                           where info.Name == targetName
                           && info.PropertyType == value.GetType()
                           && info.CanWrite
                           select info).FirstOrDefault();

            if (property != null)
            {
                property.SetValue(_inner, value, null);
                return true;
            }

            return false;
        }
    }

    public class PassthroughSetMemberBinder : SetMemberBinder
    {
        public override DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value, DynamicMetaObject errorSuggestion)
        {
            return null;
        }

        public PassthroughSetMemberBinder(string name, bool ignoreCase) : base(name, ignoreCase) { }
    }

    public class PassthroughGetMemberBinder : GetMemberBinder
    {

        public PassthroughGetMemberBinder(string name, bool ignoreCase) : base(name, ignoreCase) { }

        public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
        {
            return null;
        }
    }
}
