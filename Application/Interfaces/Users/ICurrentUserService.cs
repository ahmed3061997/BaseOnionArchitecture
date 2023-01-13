using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Users;

namespace Application.Interfaces.Users
{
    public interface ICurrentUserService
    {
        Task<CurrentUserDto> GetCurrentUser();
        string GetCurrentUserId();
    }
}
