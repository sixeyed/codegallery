using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;

namespace Sixeyed.CodeSamples.EntityTagger
{
    public static class EntityTagger
    {
        public static void SetETag<TEntity>(TEntity entity, Expression<Func<TEntity, string>> eTagPropertyAccessor)
        {
            var eTagProperty = AsPropertyInfo(eTagPropertyAccessor);
            var originalETag = eTagProperty.GetValue(entity, null);
            try
            {
                ResetETag(entity, eTagPropertyAccessor);
                string json;
                var serializer = new DataContractJsonSerializer(entity.GetType());
                using (var stream = new MemoryStream())
                {
                    serializer.WriteObject(stream, entity);
                    json = Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length);
                }
                var guid = GetDeterministicGuid(json);
                eTagProperty.SetValue(entity, guid.ToString(), null);
            }
            catch (Exception ex)
            {
                //TODO - logging 
                eTagProperty.SetValue(entity, originalETag, null);
            }
        }

        public static bool HasChanges<TEntity>(TEntity entity, Expression<Func<TEntity, string>> eTagPropertyAccessor)
        {
            var eTagProperty = AsPropertyInfo(eTagPropertyAccessor);
            var originalETag = eTagProperty.GetValue(entity, null);
            SetETag(entity, eTagPropertyAccessor);
            var newETag = eTagProperty.GetValue(entity, null);
            eTagProperty.SetValue(entity, originalETag, null);
            return (newETag != originalETag);
        }

        public static void ResetETag<TEntity>(TEntity entity, Expression<Func<TEntity, string>> eTagPropertyAccessor)
        {
            var eTagProperty = AsPropertyInfo(eTagPropertyAccessor);
            eTagProperty.SetValue(entity, Guid.Empty.ToString(), null);
        }

        private static Guid GetDeterministicGuid(string input)
        {
            //use MD5 hash to get a 16-byte hash of the string:
            var provider = new MD5CryptoServiceProvider();
            var inputBytes = Encoding.Default.GetBytes(input);
            var hashBytes = provider.ComputeHash(inputBytes);
            return new Guid(hashBytes);
        }

        private static PropertyInfo AsPropertyInfo(LambdaExpression expression)
        {
            PropertyInfo info = null;
            MemberExpression memberExpression = null;
            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression)expression.Body;
                memberExpression = body.Operand as MemberExpression;
            }
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }
            if (memberExpression != null)
            {
                info = (PropertyInfo)memberExpression.Member;
            }
            return info;
        }
    }
}