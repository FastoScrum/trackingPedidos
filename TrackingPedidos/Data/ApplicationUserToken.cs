using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TrackingPedidos.Data
{
    public class ApplicationUserToken : IdentityUserToken<string>
    {
        [Key]
        public int Id { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}