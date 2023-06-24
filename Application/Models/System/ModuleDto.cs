using Application.Models.Common;
using Domain.Enums;

namespace Application.Models.System
{
    public class ModuleDto
    {
        public int? Id { get; set; }
        public Modules Code { get; set; }
        public string Name { get; set; }
        public IEnumerable<CultureLookupDto> Names { get; set; }
    }
}
