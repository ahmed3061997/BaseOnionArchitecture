using Application.Models.Common;
using Domain.Enums;

namespace Application.Models.System
{
    public class OperationDto
    {
        public Guid Id { get; set; }
        public Operations Code { get; set; }
        public string Name { get; set; }
        public IEnumerable<CultureLookupDto> Names { get; set; }
    }
}
