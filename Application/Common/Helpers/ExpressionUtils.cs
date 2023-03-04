using Application.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Helpers
{
    public static class ExpressionUtils
    {
        public static Expression<Func<T, object>> BuildSortExpression<T>(string propertyName)
        {
            var elementType = typeof(T);
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = propertyName.Split('.').Aggregate((Expression)parameter, Expression.Property);
            return Expression.Lambda<Func<T, object>>(property, parameter);
        }

        public static Expression<Func<T, bool>> BuildPredicate<T>(string propertyName, string comparison, string value)
        {
            var type = typeof(T);
            var path = propertyName.Split('.');

            Expression left;
            var parameter = Expression.Parameter(typeof(T), "x");

            if (path.Length == 1)
            {
                left = Expression.Property(parameter, propertyName);
            }
            else if (path.Length == 2)
            {
                var propInfo = type.GetProperty(path[0]);
                if (propInfo.PropertyType.IsPrimitive)
                {
                    left = propertyName.Split('.').Aggregate((Expression)parameter, Expression.Property);
                }
                else
                {
                    var nestedType = propInfo.PropertyType.GenericTypeArguments[0];

                    var nestedParameter = Expression.Parameter(nestedType, "y");
                    var predicate = Expression.Lambda(
                        MakeComparison(Expression.Property(nestedParameter, path[1]), comparison, value),
                        nestedParameter);
                    left = Expression.Call(
                          typeof(Enumerable),
                          "Any",
                          new[] { nestedType },
                          Expression.Property(parameter, path[0]),
                          predicate);

                    return Expression.Lambda<Func<T, bool>>(left, parameter);
                }
            }
            else
            {
                throw new NotSupportedException("Property navigation is not supported");
            }

            var body = MakeComparison(left, comparison, value);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        private static Expression MakeComparison(Expression left, string comparison, string value)
        {
            switch (comparison)
            {
                case "==":
                    return MakeBinary(ExpressionType.Equal, left, value);
                case "!=":
                    return MakeBinary(ExpressionType.NotEqual, left, value);
                case ">":
                    return MakeBinary(ExpressionType.GreaterThan, left, value);
                case ">=":
                    return MakeBinary(ExpressionType.GreaterThanOrEqual, left, value);
                case "<":
                    return MakeBinary(ExpressionType.LessThan, left, value);
                case "<=":
                    return MakeBinary(ExpressionType.LessThanOrEqual, left, value);
                case "Contains":
                case "StartsWith":
                case "EndsWith":
                    return Expression.Call(MakeString(left), comparison, Type.EmptyTypes, Expression.Constant(value, typeof(string)));
                default:
                    throw new NotSupportedException($"Invalid comparison operator '{comparison}'.");
            }
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
                        valueType == typeof(Guid) ? Guid.Parse(value) :
                        Convert.ChangeType(value, valueType);
                }
            }
            var right = Expression.Constant(typedValue, left.Type);
            return Expression.MakeBinary(type, left, right);
        }
    }
}
