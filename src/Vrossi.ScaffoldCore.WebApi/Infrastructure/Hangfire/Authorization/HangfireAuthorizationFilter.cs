using Hangfire.Dashboard;
using System.Net.Http.Headers;
using System.Text;

namespace Vrossi.ScaffoldCore.WebApi.Infrastructure.Hangfire.Authorization
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly HangfireAuthorizationOptions _options;
        public HangfireAuthorizationFilter(HangfireAuthorizationOptions options)
        {
            _options = options;
        }
        private bool Challenge(HttpContext context)
        {
            context.Response.StatusCode = 401;
            context.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Hangfire Dashboard\"");
            return false;
        }
        public bool Authorize(DashboardContext _context)
        {
            var context = _context.GetHttpContext();

            if (_options.SslRedirect && context.Request.Scheme != "https")
            {
                string redirectUri = new UriBuilder("https", context.Request.Host.ToString(), 443, context.Request.Path).ToString();

                context.Response.StatusCode = 301;
                context.Response.Redirect(redirectUri);
                return false;
            }

            if (!_options.RequireSsl && !context.Request.IsHttps)
                return false;

            var header = context.Request.Headers["Authorization"];

            if (!string.IsNullOrWhiteSpace(header))
            {
                var authValues = AuthenticationHeaderValue.Parse(header);

                if ("Basic".Equals(authValues.Scheme, StringComparison.OrdinalIgnoreCase))
                {
                    var parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authValues.Parameter));
                    var parts = parameter.Split(':');

                    if (parts.Length > 1)
                    {
                        var login = parts[0];
                        var password = parts[1];

                        if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password))
                        {
                            return _options
                                .Users
                                .Any(user => user.Validate(login, password, _options.LoginCaseSensitive))
                                   || Challenge(context);
                        }
                    }
                }
            }

            return Challenge(context);
        }
    }
}
