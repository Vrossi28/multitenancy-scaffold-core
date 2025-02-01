using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Vrossi.ScaffoldCore.Core.Security;
using Vrossi.ScaffoldCore.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.WebApi.Infrastructure.Authentication
{
    public class JwtAuthManager : ITokenProvider
    {
        private JwtAuthManager() { }

        private readonly JwtTokenOptions _options;
        public readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly SigningCredentials _signingCredentials;

        public JwtAuthManager(IOptions<JwtTokenOptions> options)
        {
            _options = options.Value;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecretKey));
            _signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }
        public string GenerateToken(Account account)
        {
            var claims = GetClaims(account);
            var token = new JwtSecurityToken(_options.Issuer, _options.Audience, claims, signingCredentials: _signingCredentials, expires: DateTime.UtcNow.AddHours(_options.ExpiryHours));
            return _jwtSecurityTokenHandler.WriteToken(token);
        }
        private List<Claim> GetClaims(Account account)
        {
            var claims = new List<Claim>(5)
            {
                new Claim(AppClaimTypes.Name, account.Name, AppClaimValueTypes.String),
                new Claim(AppClaimTypes.Email,account.Email, AppClaimValueTypes.Email),
                new Claim(AppClaimTypes.UserId, account.Id.ToString()),
                new Claim(AppClaimTypes.IsAdmin, account.Admin.ToString(), AppClaimValueTypes.Boolean),
                new Claim(AppClaimTypes.DefaultTenantId, account.Default.Id.ToString(), AppClaimValueTypes.String),
            };

            if (!account.Admin)
            {
                var roles = account.Profiles.ToDictionary(x => x.Tenant.Id, x => x.Profile.ToString());
                var rolesJson = JsonSerializer.Serialize(roles);
                claims.Add(new Claim(AppClaimTypes.Role, rolesJson, JsonClaimValueTypes.Json));
            }

            return claims;
        }
    }
}
