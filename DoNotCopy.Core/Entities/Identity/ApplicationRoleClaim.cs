using Microsoft.AspNetCore.Identity;

namespace DoNotCopy.Core.Entities.Identity
{
    public class ApplicationRoleClaim : IdentityRoleClaim<int>
    {
        public virtual ApplicationRole Role { get; set; }
    }
}
