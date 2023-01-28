using Domain.Common;

namespace Domain.Entities.System
{
    public class ModuleName : BaseEntity
    {
        public Guid ModuleId { get; set; }
        public string Culture { get; set; }
        public string Name { get; set; }
    }
}
