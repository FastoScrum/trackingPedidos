using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TrackingPedidos.Data
{
    public class ApplicationUserLogin : IdentityUserLogin<string>
    {
        [Key]
        public int Id { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}