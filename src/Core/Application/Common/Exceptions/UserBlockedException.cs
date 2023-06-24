namespace Application.Common.Exceptions
{
    public class UserBlockedException : Exception
    {
        public UserBlockedException() : base("Your account is locked") { }
        public UserBlockedException(string message) : base(message) { }
    }
}
