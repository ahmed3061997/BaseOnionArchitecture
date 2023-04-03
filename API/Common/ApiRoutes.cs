namespace API.Common
{
    public static class ApiRoutes
    {
        // System
        public const string GetCultures = "get-cultures";

        // Users
        public const string Login = "login";
        public const string Logout = "logout";
        public const string Register = "register";
        public const string RefreshToken = "refresh-token";
        public const string ConfirmEmail = "confirm-email";
        public const string SendEmailConfirmation = "send-email-confirmation";
        public const string ResetPassword = "reset-password";
        public const string SendResetPassword = "send-reset-password";
        public const string Authenticate = "authenticate";
        public const string GetCurrentUser = "get-current-user";
        public const string GetClaims = "get-claims";
        public const string GetRoles = "get-roles";

        // General
        public const string Import = "import";
        public const string Export = "export";
        public const string MigrateData = "migrate-data";
        public const string GetCodes = "get-codes";
        public const string Get = "get";
        public const string GetDrop = "get-drop";
        public const string GetAll = "get-all";
        public const string Create = "create";
        public const string Edit = "edit";
        public const string Delete = "delete";
    }
}
