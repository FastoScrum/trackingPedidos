using System;
using System.ComponentModel.DataAnnotations;

namespace TrackingPedidos.Models
{
    public partial class Pedidos
    {
        [Key]
        public int PedId { get; set; }
        public string InvoiceNumber { get; set; }
        public int CliId { get; set; }

        [DataType(DataType.Currency)]
        public decimal PedTotal { get; set; }

        public DateTime PedFechaEnvio { get; set; }
        public char PedFase { get; set; }
        public string PedLugarOrigen { get; set; }
        public string PedLugarDestino { get; set; }
        public string PedDireccionDestino { get; set; }
        public bool PedEnvioEstandar { get; set; }

        [DataType(DataType.Currency)]
        public decimal PedCostoExtra { get; set; }

        public bool PedRegalo { get; set; }
        public DateTime PedFechaDespachado { get; set; }
        public DateTime? PedFechaCamino { get; set; }
        public DateTime? PedFechaFin { get; set; }
        public string PedDescripcion { get; set; }

        public Entregas Entregas { get; set; }
    }
}