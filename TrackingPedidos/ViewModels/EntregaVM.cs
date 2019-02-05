using System.ComponentModel.DataAnnotations;
using TrackingPedidos.Validators;

namespace TrackingPedidos.ViewModels
{
    public class EntregaVM
    {
        [Display(Name = "Entregado")]
        public bool Estado { get; set; }

        [Display(Name = "Persona presente")]
        public bool PersonaPresente { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Parentesco es obligatorio.")]
        public string Parentesco { get; set; }

        [Required(ErrorMessage = "Apellidos son obligatorios.")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "Nombres son obligatorios.")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Celular es obligatorio.")]
        [DataType(DataType.PhoneNumber)]
        public string Celular { get; set; }

        [Required(ErrorMessage = "Cédula es obligatorio.")]
        [Display(Name = "Cédula")]
        [Identification]
        public string Identificacion { get; set; }

        [Display(Name = "Dirección")]
        public string Direccion { get; set; }
    }
}