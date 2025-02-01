using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;

namespace Vrossi.ScaffoldCore.WebApi.Infrastructure.Hangfire
{
    public class HangfireCultureFilter : JobFilterAttribute, IClientFilter
    {
        public string Culture { get; set; }
        public string UiCulture { get; set; }

        public HangfireCultureFilter()
        {
            Order = 0;
        }

        public void OnCreated(CreatedContext filterContext) { }
        public void OnCreating(CreatingContext filterContext)
        {
            filterContext.SetJobParameter("CurrentCulture", Culture);
            filterContext.SetJobParameter("CurrentUICulture", UiCulture);
        }
    }
}
