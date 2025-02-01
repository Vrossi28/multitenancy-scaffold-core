using Vrossi.ScaffoldCore.Infrastructure.Domain;

namespace Vrossi.ScaffoldCore.WebApi.Infrastructure.Authentication
{
    public interface ITokenProvider
    {
        string GenerateToken(Account account);
    }
}
