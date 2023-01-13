using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Common
{
    public class EmailMessage
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool IsHtml { get; set; }  

        public EmailMessage(IDictionary<string, string> to, string subject, string content, bool isHtml = true)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x.Key, x.Value)));
            Subject = subject;
            Content = content;
            IsHtml = isHtml;
        }
    }
}
