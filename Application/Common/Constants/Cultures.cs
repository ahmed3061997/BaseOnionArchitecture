using Application.Models.System;

namespace Application.Common.Constants
{
    public static class Cultures
    {
        public const string Arabic = "ar";
        public const string English = "en";

        public static readonly IEnumerable<CultureDto> All = new List<CultureDto>()
        {
            new CultureDto() { Code = Arabic, Name = "عربي", Flag = "eg"},
            new CultureDto() { Code = English, Name = "English", Flag = "us", IsDefault = true},
        };
    }
}
