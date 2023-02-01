using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Users
{
    public class ApplicationRoleName : BaseEntity
    {
        [ForeignKey("Role")]
        public string RoleId { get; set; }
        public string Culture { get; set; }
        public string Name { get; set; }

        public ApplicationRole Role { get; set; }
    }
}
