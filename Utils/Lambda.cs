using System;
using System.Linq.Expressions;

namespace Utils
{
    public static class Lambda
    {
        public static string GetPropertyName<TViewModel, TValue>(this Expression<Func<TViewModel, TValue>> Property)
        {
            var expression = (MemberExpression)Property.Body;
            return expression.Member.Name;
        }
    }
}
