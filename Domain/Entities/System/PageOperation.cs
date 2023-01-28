using Domain.Common;

namespace Domain.Entities.System
{
    public class PageOperation : BaseEntity
    {
        public Guid PageId { get; set; }
        public Guid OperationId { get; set; }

        public Page Page { get; set; }
        public Operation Operation { get; set; }
    }
}
