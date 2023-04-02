using Application.Common.Constants;
using Application.Common.Helpers;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Application.Common.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> IncludeIf<T, TProperty>(this IQueryable<T> source, bool condition,
        Expression<Func<T, TProperty>> navigationPropertyPath) where T : class
        {
            if (!condition) return source;
            return source.Include(navigationPropertyPath);
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> source, IEnumerable<string> propertyNames, string? value)
        {
            if (value == null) return source;
            return source.Where(ExpressionUtils.BuildPredicate<T>(propertyNames, "Contains", value));
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> source, string? propertyName, string? value)
        {
            if (propertyName == null || value == null) return source;
            return source.Where(ExpressionUtils.BuildPredicate<T>(propertyName, "Contains", value));
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> source, string? propertyName, string comparison, string? value)
        {
            if (propertyName == null || value == null) return source;
            return source.Where(ExpressionUtils.BuildPredicate<T>(propertyName, comparison, value));
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string? propertyName, SortDirection direction)
        {
            if (propertyName == null || direction == SortDirection.None) return source;

            var expression = ExpressionUtils.BuildSortExpression<T>(propertyName);
            if (direction == SortDirection.Ascending)
                return source.OrderBy(expression);
            else
                return source.OrderByDescending(expression);
        }

        public static IQueryable<T> OrderByPredicated<T>(this IQueryable<T> source, string? orderPropertyName, string predicatePropertyName, string value, SortDirection direction)
        {
            if (orderPropertyName == null || direction == SortDirection.None) return source;

            var expression = ExpressionUtils.BuildPredicatedSortExpression<T>(orderPropertyName, predicatePropertyName, "==", value);
            if (direction == SortDirection.Ascending)
                return source.OrderBy(expression);
            else
                return source.OrderByDescending(expression);
        }
    }
}
