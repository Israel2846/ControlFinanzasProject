using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ControlFinanzasProject.Models
{
    public class PagoTarjeta
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime FechaPago { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto del pago debe ser mayor que cero.")]
        public decimal MontoPago { get; set; }
        [Required]
        public bool EsActivo { get; set; } = true;
        // foreign key
        [Required]
        public int TarjetaCreditoId { get; set; }
        [Required]
        public int MovimientoId { get; set; }
        // Navigation property
        [ForeignKey("TarjetaCreditoId")]
        public virtual TarjetaCredito TarjetaCredito { get; set; } = null!;
        [ForeignKey("MovimientoId")]
        public virtual Movimiento Movimiento { get; set; } = null!;
    }
}