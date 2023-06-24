using Domain.Common;
using Domain.Enums;

namespace Domain.Entities.System
{
    public class Operation : BaseEntity
    {
        public Operations Code { get; set; }

        public IList<OperationName> Names { get; set; }
    }
}
