using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Notifications.Roles
{
    public class RoleBlockedNotification : BaseNotification
    {
        private const string SUBJECT = "ROLE_BLOCKED";

        public RoleBlockedNotification(IEnumerable<string>? targetUsers)
            : base(targetUsers, SUBJECT, null)
        {
        }

        public RoleBlockedNotification(IEnumerable<string>? targetUsers, object? data)
            : base(targetUsers, SUBJECT, data)
        {
        }
    }
}
