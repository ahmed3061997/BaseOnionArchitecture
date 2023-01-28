namespace Application.Common.Exceptions
{
    public class NameUsedException : Exception
    {
        public IEnumerable<string>? Names { get; }

        public NameUsedException() : base("Name used before.") { }

        public NameUsedException(string message) : base(message) { }

        public NameUsedException(IEnumerable<string> names) : base("Name used before.")
        {
            Names = names;
        }

        public NameUsedException(string message, IEnumerable<string> names) : base(message)
        {
            Names = names;
        }
    }
}
