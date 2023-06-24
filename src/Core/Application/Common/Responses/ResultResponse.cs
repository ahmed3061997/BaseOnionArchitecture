namespace Application.Common.Responses
{
    public class ResultResponse<T> : Response, IResultResponse<T>
    {
        public T? Value { get; set; }
    }
}
