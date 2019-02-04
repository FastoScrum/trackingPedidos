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

        public string Parentesco { get; set; }
        public string Apellidos { get; set; }
        public string Nombres { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Celular { get; set; }

        [Display(Name = "Cédula")]
        [Identification]
        public string Identificacion { get; set; }

        [Display(Name = "Dirección")]
        public string Direccion { get; set; }
    }
}