using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Common.Responses
{
    public class Response : IResponse
    {
        public bool Result { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
