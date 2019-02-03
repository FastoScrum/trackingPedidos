using System;
using System.ComponentModel.DataAnnotations;

namespace TrackingPedidos.Models
{
    public partial class Pedidos
    {
        [Key]
        public int PedId { get; set; }

        [Display(Name = "Número Factura")]
        public string InvoiceNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        public string ClienteEmail { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Total")]
        public decimal PedTotal { get; set; }

        [Display(Name = "Fecha envio")]
        public DateTime PedFechaEnvio { get; set; }

        public char PedFase { get; set; }

        [Display(Name = "Lugar Origen")]
        public string PedLugarOrigen { get; set; }

        [Display(Name = "Lugar Destino")]
        public string PedLugarDestino { get; set; }

        public string PedDireccionEntrega { get; set; }
        public bool PedEnvioEstandar { get; set; }

        [DataType(DataType.Currency)]
        public decimal PedCostoExtra { get; set; }

        public bool PedRegalo { get; set; }
        public DateTime? PedFechaDespachado { get; set; }
        public DateTime? PedFechaCamino { get; set; }
        public DateTime? PedFechaFin { get; set; }
        public DateTime? PedFechaEntrega { get; set; }
        public string PedDescripcion { get; set; }
        public string PedTarjeta { get; set; }

        public Entregas Entregas { get; set; }

        public string FullEstado
        {
            get
            {
                if (this.PedFase == 'P')
                {
                    return "Pendiente";
                }
                else if (this.PedFase == 'I')
                {
                    return "Despachado";
                }
                else if (this.PedFase == 'C')
                {
                    return "En Camino";
                }
                else if (this.PedFase == 'F')
                {
                    return "En Distribuidora";
                }
                else if (this.PedFase == 'E')
                {
                    return "Entregado";
                }
                else if (this.PedFase == 'N')
                {
                    return "No Entregado";
                }
                return null;
            }
        }
    }
}