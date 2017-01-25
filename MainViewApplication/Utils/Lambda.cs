using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
