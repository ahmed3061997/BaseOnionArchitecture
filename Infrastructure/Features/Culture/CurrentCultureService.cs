using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Application.Interfaces.Culture;

namespace Infrastructure.Features.Culture
{
    public class CurrentCultureService : ICurrentCultureService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentCultureService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentCulture()
        {
            var requestCulture = GetRequestCulture();
            return requestCulture?.Culture.ToString();
        }

        public string GetCurrentUICulture()
        {
            var requestCulture = GetRequestCulture();
            return requestCulture?.UICulture.ToString();
        }

        private RequestCulture GetRequestCulture()
        {
            var feature = httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>();
            return feature?.RequestCulture;
        }
    }
}
