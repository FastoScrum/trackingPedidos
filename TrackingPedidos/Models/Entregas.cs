using System.ComponentModel.DataAnnotations;

namespace TrackingPedidos.Models
{
    public partial class Entregas
    {
        [Key]
        public int EntId { get; set; }

        public int PedId { get; set; }
        public string EntPerApellidos { get; set; }
        public string EntPerNombres { get; set; }
        public string EntPerIdentificacion { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string EntCelular { get; set; }

        public Pedidos Ped { get; set; }
    }
}