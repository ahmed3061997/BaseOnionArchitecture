using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Notifications
{
    public interface INotificationService
    {
        Task Push(INotification notification);
    }
}
