using Vrossi.ScaffoldCore.Core.Domain;
using Vrossi.ScaffoldCore.Core.Enums;
using Vrossi.ScaffoldCore.Infrastructure.Extensions;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace Vrossi.ScaffoldCore.Infrastructure.Domain
{
    public class Account : AggregateRoot
    {
        protected Account() { }
        public Account(string name, string lastName,  string email, string password) : base(Guid.NewGuid())
        {
            SetName(name);
            SetLastName(lastName);
            SetEmail(email);
            SetPassword(password);
        }

        public Account(string name, string lastName, string email, string password, bool admin, Tenant tenant) : base(Guid.NewGuid())
        {
            SetName(name);
            SetLastName(lastName);
            SetEmail(email);
            SetPassword(password);
            ChangeDefault(tenant);
            Admin = admin;
            if(admin)
                AddProfile(RoleType.Administrator, tenant.Id);
        }

        public Account AddProfile(RoleType profile, Guid tenantId)
        {
            var accountProfile = new AccountProfile(profile, tenantId);
            return Add(accountProfile);
        }

        private Account Add(AccountProfile accountProfile)
        {
            AssertionConcern.AssertArgumentNotNull(accountProfile, "Account Profile is mandatory");

            if (Profiles.Any(x => x.Profile == accountProfile.Profile && x.TenantId == accountProfile.TenantId))
                throw new DomainException($"Profile {accountProfile.Profile.GetDisplayName()} already added");

            _profiles.Add(accountProfile);

            return this;
        }

        private List<AccountProfile> _profiles = new();
        public IReadOnlyCollection<AccountProfile> Profiles => _profiles.AsReadOnly();

        public string Name { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public Tenant Default { get; private set; }
        public DateTime? LastLoginDate { get; private set; }
        public string VerificationToken { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public string PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpiration { get; set; }
        public bool Admin { get; private set; }

        public void ChangeDefault(Tenant tenant)
        {
            AssertionConcern.AssertArgumentNotNull(tenant, "Default tenant is mandatory");
            Default = tenant;
        }

        public void SetName(string name)
        {
            AssertionConcern.AssertArgumentNotEmpty(name, "Name is mandatory");
            Name = name;
        }

        public void SetLastName(string lastName)
        {
            AssertionConcern.AssertArgumentNotEmpty(lastName, "Last Name is mandatory");
            LastName = lastName;
        }

        public void SetEmail(string email)
        {
            AssertionConcern.AssertArgumentNotEmpty(email, "Email is mandatory");
            AssertionConcern.AssertValidEmail(email, "Not a valid email");

            Email = email;
        }

        public void UpdateLastLogin()
        {
            AssertionConcern.AssertArgumentNotEmpty(Password, "It is not possible to update the last login date without having a Password defined");
            LastLoginDate = DateTime.Now;
        }

        public void SetPassword(string password)
        {
            AssertionConcern.AssertArgumentNotEmpty(password, "Password is mandatory");
            Password = BC.HashPassword(password);
        }

        public void ChangePassword(string current, string newPwd, string confirmation)
        {
            AssertionConcern.AssertArgumentNotEmpty(current, "Current Password is mandatory");
            AssertionConcern.AssertArgumentNotEmpty(newPwd, "New Password is mandatory");
            AssertionConcern.AssertArgumentNotEmpty(confirmation, "Confirmation is mandatory");

            if (!newPwd.Equals(confirmation))
                throw new DomainException("New Password does not mach with Confirmation");

            if (!BC.Verify(current, Password))
                throw new DomainException("Current password does not match with registered password");

            if (BC.Verify(newPwd, Password))
                throw new DomainException("New password can not be equal current password");

            Password = BC.HashPassword(newPwd);
        }
    }
}
