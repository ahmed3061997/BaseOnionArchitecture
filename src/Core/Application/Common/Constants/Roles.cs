namespace Application.Common.Constants
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Developer = "Developer";

        public static Dictionary<string, string> ArDic = new Dictionary<string, string>()
        {
            {Admin, "مدير النظام"},
            {Developer, "مطور النظام"}
        };
    }
}
