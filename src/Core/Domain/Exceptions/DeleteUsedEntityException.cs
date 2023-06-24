namespace Domain.Exceptions
{
    public class DeleteUsedEntityException : Exception
    {
        public DeleteUsedEntityException() : base("Can not delete a used entity.") { }
        public DeleteUsedEntityException(string message) : base(message) { }
    }
}
