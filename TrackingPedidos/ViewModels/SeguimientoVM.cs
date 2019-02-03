using System.ComponentModel.DataAnnotations;

namespace TrackingPedidos.ViewModels
{
    public class SeguimientoVM
    {
        [Display(Name = "Número Factura")]
        public string InvoiceNumber { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio.")]
        [Display(Name = "Lugar Origen")]
        public string PedLugarOrigen { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio.")]
        [Display(Name = "Lugar Destino")]
        public string PedLugarDestino { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Total")]
        public decimal PedTotal { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio.")]
        [Display(Name = "Tipo de envío")]
        public string PedTipoEnvio { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio.")]
        [Display(Name = "Regalo")]
        public string PedRegalo { get; set; }

        [Display(Name = "Tarjeta")]
        public string PedTarjeta { get; set; }
    }
}