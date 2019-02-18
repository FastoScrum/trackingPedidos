using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrackingPedidos.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "Usuario")]
        public override string UserName { get; set; }

        [Required]
        [EmailAddress]
        public override string Email { get; set; }

        [Display(Name = "Fecha Desbloqueo")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public override DateTimeOffset? LockoutEnd { get; set; }

        [PersonalData]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [PersonalData]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
        public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
        public virtual ICollection<ApplicationUserToken> Tokens { get; set; }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}