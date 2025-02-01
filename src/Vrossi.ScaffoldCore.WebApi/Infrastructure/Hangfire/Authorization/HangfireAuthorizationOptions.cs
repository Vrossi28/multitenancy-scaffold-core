namespace Vrossi.ScaffoldCore.WebApi.Infrastructure.Hangfire.Authorization
{
    public class HangfireAuthorizationOptions
    {
        public bool SslRedirect { get; set; } = true;
        public bool RequireSsl { get; set; } = true;
        public bool LoginCaseSensitive { get; set; } = true;
        public List<HangfireUser> Users { get; set; } = new List<HangfireUser>();
    }
}
