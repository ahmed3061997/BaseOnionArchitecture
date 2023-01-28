namespace Application.Common.Constants
{
    public static class Cultures
    {
        public const string Arabic = "ar";
        public const string English = "en";

        public static readonly Dictionary<string, string> Flags = new Dictionary<string, string>()
        {
            { Arabic, "egypt" },
            { English, "united-states" },
        };

        public static readonly Dictionary<string, string> Names = new Dictionary<string, string>()
        {
            { Arabic, "عربي" },
            { English, "English" },
        };
    }
}
