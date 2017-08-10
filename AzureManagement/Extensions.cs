using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

namespace AzureManagement
{
    public static class Extensions
    {
        //var query = context.Persons.Where(userFilters);

        public static Expression<Func<TInput, bool>> CreateFilterExpression<TInput>(IEnumerable<Filter> filters)
        {
            var param = Expression.Parameter(typeof(TInput), "");
            Expression lambdaBody = null;
            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    Expression expression = Expression.Equal(Expression.Property(param, filter.FieldName), Expression.Constant(filter.FilterString));
                    lambdaBody = lambdaBody == null ? expression : Expression.Or(lambdaBody, expression);
                }
            }

            return lambdaBody == null ? Expression.Lambda<Func<TInput, bool>>(Expression.Constant(false)) : Expression.Lambda<Func<TInput, bool>>(lambdaBody, param);
        }

        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, IEnumerable<Filter> filters)
        {
            return source.Where(CreateFilterExpression<T>(filters).Compile());
        }

        public static IReadOnlyCollection<T> Where<T>(this IReadOnlyCollection<T> source, IReadOnlyCollection<Filter> filters)
        {
            return source.Where(CreateFilterExpression<T>(filters).Compile()).ToArray();
        }




        public static IQueryable<T> Where<T>(this IQueryable<T> source, IEnumerable<Filter> filters)
        {
            return source.Where(CreateFilterExpression<T>(filters));
        }



    }

    public class Filter
    {
        public string FieldName { get; set; }
        public string FilterString { get; set; }
    }
}
