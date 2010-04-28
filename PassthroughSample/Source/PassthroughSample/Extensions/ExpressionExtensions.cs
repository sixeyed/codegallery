using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace PassthroughSample
{
    /// <summary>
    /// Extensions to <see cref="LambdaExpression"/>
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Returns the <see cref="MemberInfo"/> referenced by the expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MemberInfo AsMemberInfo(this LambdaExpression expression)
        {
            MemberInfo info = null;
            MemberExpression memberExpression = null;
            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression)expression.Body;
                memberExpression = body.Operand as MemberExpression;
                info = memberExpression.Member;
            }
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
                info = memberExpression.Member;
            }
            else if (expression.Body.NodeType == ExpressionType.Call)
            {
                var exp = expression.Body as MethodCallExpression;
                info = exp.Method;
            }
            return info;
        }

        /// <summary>
        /// Returns the name of the member referenced by the expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string AsMemberName(this LambdaExpression expression)
        {
            var info = expression.AsMemberInfo();
            return info == null ? null : info.Name;
        }
    }
}
