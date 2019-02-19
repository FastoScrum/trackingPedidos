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

        [Display(Name = "Fecha de envío")]
        public DateTime PedFechaEnvio { get; set; }

        [Display(Name = "Estado")]
        public char PedFase { get; set; }

        [Display(Name = "Lugar Origen")]
        public string PedLugarOrigen { get; set; }

        [Display(Name = "Lugar Destino")]
        public string PedLugarDestino { get; set; }

        public string PedDireccionEntrega { get; set; }

        [Display(Name = "Tipo de envío")]
        public bool PedEnvioEstandar { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Costo extra")]
        public decimal PedCostoExtra { get; set; }

        [Display(Name = "Regalo")]
        public bool PedRegalo { get; set; }

        [Display(Name = "Fecha despachado")]
        public DateTime? PedFechaDespachado { get; set; }

        [Display(Name = "Fecha en camino")]
        public DateTime? PedFechaCamino { get; set; }

        [Display(Name = "Fecha finalizado")]
        public DateTime? PedFechaFin { get; set; }

        [Display(Name = "Fecha entrega")]
        public DateTime? PedFechaEntrega { get; set; }

        [Display(Name = "Descripción")]
        public string PedDescripcion { get; set; }

        [Display(Name = "Tarjeta")]
        public string PedTarjeta { get; set; }

        public string Despachador { get; set; }
        public string Transportista { get; set; }
        public string Distribuidor { get; set; }
        public string Mensajero { get; set; }

        public Entregas Entregas { get; set; }

        public string TipoEnvio
        {
            get
            {
                return this.PedEnvioEstandar ? "Estándar" : "Especial";
            }
        }

        public string TipoRegalo
        {
            get
            {
                return this.PedRegalo ? "Sí" : "No";
            }
        }

        public int TiempoEstimado
        {
            get
            {
                if (this.PedFechaCamino != null)
                {
                    var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
                    var diferencia = (now - this.PedFechaCamino).Value.TotalDays;
                    if (this.PedEnvioEstandar)
                    {
                        var est = (int)Math.Round(7 - diferencia);
                        return est < 0 ? 0 : est;
                    }
                    else
                    {
                        var est = (int)Math.Round(2 - diferencia);
                        return est < 0 ? 0 : est;
                    }
                }
                return 0;
            }
        }

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