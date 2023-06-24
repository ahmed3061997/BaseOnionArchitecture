using System.Reflection;

namespace Application.Common.Extensions
{
    public static class Extensions
    {
        public static IEnumerable<TResult> GetStaticMembers<TResult>(Type type)
        {
            return type.GetMembers()
                .Where(m => m.MemberType == MemberTypes.Field)
                .Select(m => m.GetMemberValue(null))
                .OfType<TResult>();
        }

        public static IReadOnlyDictionary<string, TResult?> GetStaticMembersDic<TResult>(Type type)
        {
            return type.GetMembers()
                .Where(m => m.MemberType == MemberTypes.Field)
                .ToDictionary(k => k.Name, m => (TResult?)m.GetMemberValue(null));
        }

        public static object? GetMemberValue(this MemberInfo memberInfo, object? forObject)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).GetValue(forObject);
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).GetValue(forObject);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
