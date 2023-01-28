using Domain.Common;

namespace Domain.Entities.System
{
    public class PageName : BaseEntity
    {
        public Guid PageId { get; set; }
        public string Culture { get; set; }
        public string Name { get; set; }
    }
}
