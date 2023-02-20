using Application.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Emails
{
    public interface IConfirmationEmailSender
    {
        Task Send(IdentityTokenResult result, string resetUrl);
    }
}
