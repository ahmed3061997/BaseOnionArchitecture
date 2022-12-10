namespace Application.Exceptions
{
    public class CreateUserException : Exception
    {
        public CreateUserException(IEnumerable<string> errors)
        {
            Errors = errors;
        }

        public IEnumerable<string> Errors { get; }
    }
}
