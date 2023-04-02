using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Users
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public string RoleId { get; set; }
        public ApplicationRole Role { get; set; }
    }
}
