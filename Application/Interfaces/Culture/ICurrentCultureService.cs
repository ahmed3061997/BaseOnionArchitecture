using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Culture
{
    public interface ICurrentCultureService
    {
        public string GetCurrentCulture();
        public string GetCurrentUICulture();
    }
}
