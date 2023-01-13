using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Users;

namespace Application.Interfaces.Users
{
    public interface IUserService
    {
        Task<UserDto> Get(string userId);
        Task<AuthResult> Create(UserDto user, string password);
        Task AssignToRole(string userId, string role);
    }
}
