namespace Application.Common.Exceptions
{
    public class CodeUsedException : Exception
    {
        public CodeUsedException() : base("Code used before.") { }
        public CodeUsedException(string message) : base(message) { }
    }
}
