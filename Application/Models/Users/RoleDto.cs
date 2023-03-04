using Application.Models.Common;

namespace Application.Models.Users
{
    public class RoleDto
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<string> Claims { get; set; }
        public IEnumerable<CultureLookupDto> Names { get; set; }
    }
}
