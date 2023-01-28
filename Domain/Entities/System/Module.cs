using Domain.Enums;
using Domain.Common;

namespace Domain.Entities.System
{
    public class Module : BaseEntity
    {
        public Modules Code { get; set; }

        public IList<ModuleName> Names { get; set; }
        public IList<Page> Pages { get; set; }
    }
}
