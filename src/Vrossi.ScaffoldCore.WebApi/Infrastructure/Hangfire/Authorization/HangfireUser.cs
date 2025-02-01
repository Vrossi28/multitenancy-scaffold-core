using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace Vrossi.ScaffoldCore.WebApi.Infrastructure.Hangfire.Authorization
{
    public class HangfireUser
    {
        public string Login { get; set; }
        public byte[] Password { get; set; }
        public string PasswordClear
        {
            set
            {
                using var cryptoProvider = SHA1.Create();
                Password = cryptoProvider.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
        }

        public bool Validate(string login, string password, bool loginCaseSensitive)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentNullException(nameof(login));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));

            if (login.Equals(Login, loginCaseSensitive ? StringComparison.CurrentCulture : StringComparison.OrdinalIgnoreCase))
            {
                using var cryptoProvider = SHA1.Create();
                var passwordHash = cryptoProvider.ComputeHash(Encoding.UTF8.GetBytes(password));
                return StructuralComparisons.StructuralEqualityComparer.Equals(passwordHash, Password);
            }

            return false;
        }
    }
}
