using System.ComponentModel.DataAnnotations;

namespace TrackingPedidos.ViewModels
{
    public class UserVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Nombres")]
        [RegularExpression(@"^[A-ZÁÉÍÓÚÑa-záéíóúñ\s]+$", ErrorMessage = "Formato inválido de nombres.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Apellidos")]
        [RegularExpression(@"^[A-ZÁÉÍÓÚÑa-záéíóúñ\s]+$", ErrorMessage = "Formato inválido de apellidos.")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Celular")]
        [RegularExpression("^09[0-9]{8}$", ErrorMessage = "Formato inválido de celular.")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Rol { get; set; }
    }
}