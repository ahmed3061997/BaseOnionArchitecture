using Domain.Enums;
using Domain.Common;

namespace Domain.Entities.System
{
    public class Page : BaseEntity
    {
        public Guid ModuleId { get; set; }
        public Pages Code { get; set; }
        public string Url { get; set; }

        public Module Module { get; set; }
        public IList<PageName> Names { get; set; }
        public IList<PageOperation> Operations { get; set; }
    }
}
