using Microsoft.AspNetCore.Identity;

namespace DoNotCopy.Core.Entities.Identity
{
    public class ApplicationUserLogin : IdentityUserLogin<int>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
