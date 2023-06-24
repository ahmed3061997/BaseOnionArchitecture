namespace Application.Common.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<KeyValuePair<int, string>> EnumToDictionary<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
               .Cast<T>()
               .ToDictionary(t => (int)(object)t, t => t.ToString());
        }

        public static string EnumToString(this Enum value)
        {
            return Enum.GetName(value.GetType(), value);
        }
    }
}
