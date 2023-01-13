using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Users
{
    public class RoleDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Claims { get; set; }
    }
}
