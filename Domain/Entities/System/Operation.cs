using Domain.Enums;
using Domain.Common;

namespace Domain.Entities.System
{
    public class Operation : BaseEntity
    {
        public Operations Code { get; set; }

        public IList<OperationName> Names { get; set; }
    }
}
