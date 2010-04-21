using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace DynamicSP.Core.Extensions
{
    /// <summary>
    /// Extensions to <see cref="LambdaExpression"/>
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Returns the <see cref="PropertyInfo"/> referenced by the expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static PropertyInfo AsPropertyInfo(this LambdaExpression expression)
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

        /// <summary>
        /// Returns the name of the property referenced by the expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string AsPropertyName(this LambdaExpression expression)
        {
            PropertyInfo info = expression.AsPropertyInfo();
            string propertyName = info == null ? null : info.Name;
            return propertyName;
        }
    }
}
