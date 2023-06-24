using Domain.Common;
using Domain.Enums;

namespace Domain.Entities.System
{
    public class Module : BaseEntity
    {
        public Modules Code { get; set; }

        public IList<ModuleName> Names { get; set; }
        public IList<Page> Pages { get; set; }
    }
}
