using Application.Common.Constants;
using Application.Common.Helpers;

namespace Application.Common.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> source, IEnumerable<string> propertyNames, string? value)
        {
            if (value == null) return source;
            return source.Where(ExpressionHelper.BuildPredicate<T>(propertyNames, "Contains", value));
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> source, string? propertyName, string? value)
        {
            if (propertyName == null || value == null) return source;
            return source.Where(ExpressionHelper.BuildPredicate<T>(propertyName, "Contains", value));
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> source, string? propertyName, string comparison, string? value)
        {
            if (propertyName == null || value == null) return source;
            return source.Where(ExpressionHelper.BuildPredicate<T>(propertyName, comparison, value));
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string? propertyName, SortDirection direction)
        {
            if (propertyName == null || direction == SortDirection.None) return source;

            var expression = ExpressionHelper.BuildSortExpression<T>(propertyName);
            if (direction == SortDirection.Ascending)
                return source.OrderBy(expression);
            else
                return source.OrderByDescending(expression);
        }

        public static IQueryable<T> OrderByPredicated<T>(this IQueryable<T> source, string? orderPropertyName, string predicatePropertyName, string value, SortDirection direction)
        {
            if (orderPropertyName == null || direction == SortDirection.None) return source;

            var expression = ExpressionHelper.BuildPredicatedSortExpression<T>(orderPropertyName, predicatePropertyName, "==", value);
            if (direction == SortDirection.Ascending)
                return source.OrderBy(expression);
            else
                return source.OrderByDescending(expression);
        }
    }
}
