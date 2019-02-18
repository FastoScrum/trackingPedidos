using Microsoft.AspNetCore.Identity;

namespace TrackingPedidos.Data
{
    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
        public virtual ApplicationUser User { get; set; }
    }
}