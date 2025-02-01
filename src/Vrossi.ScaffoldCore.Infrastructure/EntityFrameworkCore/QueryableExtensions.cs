using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.EntityFrameworkCore
{
    public static class QueryableExtensions
    {
        private const string SORT_SEPARATOR = "|";
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string[] sortedColumns)
        {
            var expression = source.Expression;

            int count = 0;

            foreach (var sortedColumn in sortedColumns)
            {
                var parts = sortedColumn.Split(SORT_SEPARATOR);

                var parameter = Expression.Parameter(typeof(T), "x");
                var selector = Expression.PropertyOrField(parameter, parts[0]);
                var method = string.Equals(parts[1], "desc", StringComparison.OrdinalIgnoreCase) ? count == 0 ? "OrderByDescending" : "ThenByDescending" : count == 0 ? "OrderBy" : "ThenBy";
                expression = Expression.Call(typeof(Queryable), method, new Type[] { source.ElementType, selector.Type }, expression, Expression.Quote(Expression.Lambda(selector, parameter)));
                count++;
            }

            return count > 0 ? source.Provider.CreateQuery<T>(expression) : source;
        }
    }
}
