using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ControlFinanzasProject.Models
{
    public class TarjetaCredito
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El límite de crédito debe ser mayor que cero.")]
        public decimal LimiteCredito { get; set; }
        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "La deuda actual no puede ser negativa.")]
        public decimal DeudaActual { get; set; }
        [Required]
        public DateTime FechaCorte { get; set; }
        [Required]
        public DateTime FechaPago { get; set; }
        [Required]
        public bool EsActivo { get; set; } = true;
        // Navigation property
        public virtual ICollection<Movimiento> GastosCredito { get; set; } = new List<Movimiento>();
        public virtual ICollection<PagoTarjeta> Pagos { get; set; } = new List<PagoTarjeta>();
    }
}