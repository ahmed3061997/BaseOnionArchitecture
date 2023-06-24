using Domain.Common;

namespace Domain.Entities.System
{
    public class PageOperation : BaseEntity
    {
        public int PageId { get; set; }
        public int OperationId { get; set; }

        public Page Page { get; set; }
        public Operation Operation { get; set; }
    }
}
