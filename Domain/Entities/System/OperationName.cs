using Domain.Common;

namespace Domain.Entities.System
{
    public class OperationName : BaseEntity
    {
        public Guid OperationId { get; set; }
        public string Culture { get; set; }
        public string Name { get; set; }
    }
}
