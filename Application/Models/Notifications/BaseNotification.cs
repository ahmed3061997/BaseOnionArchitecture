using Application.Interfaces.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Notifications
{
    public class BaseNotification : INotification
    {
        public BaseNotification(IEnumerable<string>? targetUsers, object subject, object? data)
        {
            TargetUsers = targetUsers;
            Subject = subject;
            Data = data;
        }

        public IEnumerable<string>? TargetUsers { get; set; }
        public object Subject { get; private set; }
        public object Data { get; set; }
    }
}
