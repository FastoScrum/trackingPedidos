using System.ComponentModel.DataAnnotations;

namespace TrackingPedidos.ViewModels
{
    public class UserVM
    {
        [Required(ErrorMessage = "{0} es obligatorio.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} son obligatorios.")]
        [Display(Name = "Nombres")]
        [RegularExpression(@"^[A-ZÁÉÍÓÚÑa-záéíóúñ\s]+$", ErrorMessage = "Formato inválido de nombres.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} son obligatorios.")]
        [Display(Name = "Apellidos")]
        [RegularExpression(@"^[A-ZÁÉÍÓÚÑa-záéíóúñ\s]+$", ErrorMessage = "Formato inválido de apellidos.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio.")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Celular")]
        [RegularExpression("^09[0-9]{8}$", ErrorMessage = "Formato inválido de celular.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio.")]
        public string Rol { get; set; }

        [Required(ErrorMessage = "{0} es obligatoria.")]
        [StringLength(25, ErrorMessage = "La {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}