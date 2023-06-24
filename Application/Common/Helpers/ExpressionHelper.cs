using System.Linq.Expressions;

namespace Application.Common.Helpers
{
    public static class ExpressionHelper
    {
        public static Expression<Func<T, object>> BuildSortExpression<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = propertyName.Split('.').Aggregate((Expression)parameter, Expression.Property);
            return Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);
        }

        public static Expression<Func<T, object>> BuildPredicatedSortExpression<T>(string selectPropertyName, string predicatePropertyName, string comparison, string value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = MakePropertyOrChildSelect<T>(parameter, selectPropertyName, predicatePropertyName, comparison, value);
            return Expression.Lambda<Func<T, object>>(property, parameter);
        }

        public static Expression<Func<T, bool>> BuildPredicate<T>(IEnumerable<string> propertyNames, string comparison, string value)
        {
            return propertyNames.Select(x => BuildPredicate<T>(x, comparison, value)).Aggregate((acc, cur) =>
            {
                var parameter = acc.Parameters[0];
                var left = acc.Body;
                var right = cur.Body.ReplaceParameter(cur.Parameters[0], parameter);
                var body = Expression.Or(left, right);
                return Expression.Lambda<Func<T, bool>>(body, parameter);
            });
        }

        public static Expression<Func<T, bool>> BuildPredicate<T>(string propertyName, string comparison, string value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var left = MakePropertyOrChildPredicate<T>(parameter, propertyName, comparison, value);

            if (left.NodeType == ExpressionType.Call)
                return Expression.Lambda<Func<T, bool>>(left, parameter);

            var body = MakeComparison(left, comparison, value);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        private static Expression MakeComparison(Expression left, string comparison, string value)
        {
            return comparison switch
            {
                "==" => MakeBinary(ExpressionType.Equal, left, value),
                "!=" => MakeBinary(ExpressionType.NotEqual, left, value),
                ">" => MakeBinary(ExpressionType.GreaterThan, left, value),
                ">=" => MakeBinary(ExpressionType.GreaterThanOrEqual, left, value),
                "<" => MakeBinary(ExpressionType.LessThan, left, value),
                "<=" => MakeBinary(ExpressionType.LessThanOrEqual, left, value),
                "Contains" or "StartsWith" or "EndsWith" => Expression.Call(MakeString(left), comparison, Type.EmptyTypes, Expression.Constant(value, typeof(string))),
                _ => throw new NotSupportedException($"Invalid comparison operator '{comparison}'."),
            };
        }

        private static Expression MakeString(Expression source)
        {
            return source.Type == typeof(string) ? source : Expression.Call(source, "ToString", Type.EmptyTypes);
        }

        private static Expression MakeBinary(ExpressionType type, Expression left, string value)
        {
            object typedValue = value;
            if (left.Type != typeof(string))
            {
                if (string.IsNullOrEmpty(value))
                {
                    typedValue = null;
                    if (Nullable.GetUnderlyingType(left.Type) == null)
                        left = Expression.Convert(left, typeof(Nullable<>).MakeGenericType(left.Type));
                }
                else
                {
                    var valueType = Nullable.GetUnderlyingType(left.Type) ?? left.Type;
                    typedValue = valueType.IsEnum ? Enum.Parse(valueType, value) :
                        valueType == typeof(int) ? int.Parse(value) :
                        Convert.ChangeType(value, valueType);
                }
            }
            var right = Expression.Constant(typedValue, left.Type);
            return Expression.MakeBinary(type, left, right);
        }

        private static Expression MakePropertyOrChildPredicate<T>(ParameterExpression parameter, string propertyName, string comparison, string value)
        {
            var type = typeof(T);
            var path = propertyName.Split('.');

            if (path.Length == 1)
            {
                return Expression.Property(parameter, propertyName);
            }
            else if (path.Length == 2)
            {
                var propInfo = type.GetProperty(path[0]);
                var nestedType = propInfo.PropertyType.GenericTypeArguments[0];

                var nestedParameter = Expression.Parameter(nestedType, "y");
                var predicate = Expression.Lambda(
                    MakeComparison(Expression.Property(nestedParameter, path[1]), comparison, value),
                    nestedParameter);
                return Expression.Call(
                      typeof(Enumerable),
                      "Any",
                      new[] { nestedType },
                      Expression.Property(parameter, path[0]),
                      predicate);
            }

            throw new NotSupportedException("Property navigation is not supported");
        }

        private static Expression MakePropertyOrChildSelect<T>(ParameterExpression parameter, string selectPropertyName, string predicatePropertyName, string comparison, string value)
        {
            var type = typeof(T);
            var path = selectPropertyName.Split('.');

            if (path.Length == 1)
            {
                return Expression.Property(parameter, selectPropertyName);
            }
            else if (path.Length == 2)
            {
                var propInfo = type.GetProperty(path[0]);
                var nestedType = propInfo.PropertyType.GenericTypeArguments[0];

                var nestedParameter = Expression.Parameter(nestedType, "y");

                var predicate = Expression.Call(
                        typeof(Enumerable),
                        "FirstOrDefault",
                        new[] { nestedType },
                        Expression.Property(parameter, path[0]),
                        Expression.Lambda(
                            MakeComparison(Expression.Property(nestedParameter, predicatePropertyName), comparison, value), nestedParameter));

                return Expression.Property(predicate, path[1]);
            }

            throw new NotSupportedException("Property navigation is not supported");
        }

        private static Expression ReplaceParameter(this Expression expression, ParameterExpression source, Expression target)
        {
            return new ParameterReplacer { Source = source, Target = target }.Visit(expression);
        }

        private class ParameterReplacer : ExpressionVisitor
        {
            public ParameterExpression Source;
            public Expression Target;
            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == Source ? Target : base.VisitParameter(node);
            }
        }
    }
}
