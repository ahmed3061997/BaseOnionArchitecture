namespace Application.Common.Exceptions
{
    public class ImportException : Exception
    {
        public ImportException() : base("An error ocurred while importing data, check uploaded json file is not corrupted") { }
        public ImportException(string message) : base(message) { }
    }
}
