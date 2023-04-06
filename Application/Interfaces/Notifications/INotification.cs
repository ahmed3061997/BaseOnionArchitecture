using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Notifications
{
    public interface INotification
    {
        object Subject { get;  }
        object? Data { get; set; }
        IEnumerable<string>? TargetUsers { get; set; }
    }
}
