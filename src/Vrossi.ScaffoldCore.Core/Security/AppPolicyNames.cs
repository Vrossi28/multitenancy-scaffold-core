using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Core.Security
{
    public class AppPolicyNames
    {
        public const string Admins = "Admins";
        public const string AdminsOrDirectors = "AdminsOrDirectors";
        public const string AdminsOrDirectorsOrManagers = "AdminsOrDirectorsOrManagers";
        public const string Managers = "Managers";
        public const string Directors = "Directors";
        public const string TeamMembers = "TeamMember";
        public const string Observers = "Observer";
        public const string Everyone = "AllUsers";
    }
}
