using System.Runtime.Serialization;

namespace Application.Common.Responses
{
    public interface IResponse
    {
        public bool Result { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
