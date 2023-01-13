using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Common;

namespace Application.Interfaces.Emails
{
    public interface IEmailSender
    {
        Task Send(EmailMessage message);
    }
}
