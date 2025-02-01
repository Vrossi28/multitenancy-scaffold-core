namespace Vrossi.ScaffoldCore.WebApi.Models.Accounts
{
    public class CreateAccountModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IncludeTenant { get; set; } = false;
        public string TenantName { get; set; }
    }
}
