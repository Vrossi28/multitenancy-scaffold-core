namespace Vrossi.ScaffoldCore.WebApi.Infrastructure.Hangfire
{
    public class HangfireOptions
    {
        public string PathMath { get; set; } = "/hangfire";
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AppPath { get; set; }
        public bool IgnoreAntiforgeryToken { get; set; } = true;
        public string DashboardTitle { get; set; }
    }
}
