using AutoMapper.Internal;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Extensions
{
    public static class Extensions
    {
        public static IEnumerable<TResult> GetStaticMembers<TResult>(Type type)
        {
            return type.GetMembers()
                .Where(m => m.MemberType == System.Reflection.MemberTypes.Field)
                .Select(m => m.GetMemberValue(null))
                .OfType<TResult>();
        }

        public static IReadOnlyDictionary<string, TResult> GetStaticMembersDic<TResult>(Type type)
        {
            return type.GetMembers()
                .Where(m => m.MemberType == System.Reflection.MemberTypes.Field)
                .ToDictionary(k => k.Name, m => (TResult)m.GetMemberValue(null));
        }

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }
    }
}
