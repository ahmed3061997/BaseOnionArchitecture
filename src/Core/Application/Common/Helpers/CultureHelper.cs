using System.Globalization;

namespace Application.Common.Helpers
{
    public static class CultureHelper
    {
        private const string Arabic = "ar";

        public static string GetCurrentCulture()
        {
            return CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        }

        public static bool IsRtl()
        {
            return CultureInfo.CurrentCulture.TwoLetterISOLanguageName == Arabic;
        }
    }
}
