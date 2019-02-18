using Microsoft.AspNetCore.Identity;

namespace TrackingPedidos.Data
{
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public virtual ApplicationRole Role { get; set; }
    }
}