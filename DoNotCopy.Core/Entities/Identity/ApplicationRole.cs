using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DoNotCopy.Core.Entities.Identity
{
    public class ApplicationRole : IdentityRole<int>
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
    }
}
