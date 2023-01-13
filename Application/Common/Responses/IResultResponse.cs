namespace Application.Common.Responses
{
    public interface IResultResponse<T> : IResponse
    {
        public T? Value { get; set; }
    }
}
