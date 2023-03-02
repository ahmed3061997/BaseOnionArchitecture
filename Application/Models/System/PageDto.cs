using Application.Models.Common;
using Domain.Enums;

namespace Application.Models.System
{
    public class PageDto
    {
        public Guid? Id { get; set; }
        public Guid ModuleId { get; set; }
        public Pages Code { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public IEnumerable<CultureLookupDto> Names { get; set; }
        public IEnumerable<PageOperationDto> Operations { get; set; }
    }
}
