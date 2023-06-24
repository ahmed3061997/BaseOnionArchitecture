using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Identity
{
    [Table("AspNetRoleNames")]
    public class RoleName : BaseEntity
    {
        [ForeignKey("Role")]
        public string RoleId { get; set; }
        public string Culture { get; set; }
        public string Name { get; set; }

        public Role Role { get; set; }
    }
}
