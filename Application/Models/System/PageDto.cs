using Application.Models.Common;
using Domain.Enums;

namespace Application.Models.System
{
    public class PageDto
    {
        public Guid Id { get; set; }
        public Pages Code { get; set; }
        public IEnumerable<CultureLookupDto> Names { get; set; }
    }
}
