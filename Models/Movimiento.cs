using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ControlFinanzasProject.Models
{
    public class Movimiento
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Descripcion { get; set; } = string.Empty;
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal Monto { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public decimal SaldoAnterior { get; set; }
        [Required]
        public decimal SaldoPosterior { get; set; }
        [Required]
        public bool EsActivo { get; set; } = true;
        [Required]
        public bool EsEfectivo { get; set; } = true;
        // foreign key
        [Required]
        public int CategoriaMovimientoId { get; set; }
        public int? TarjetaCreditoId { get; set; }

        // Navigation property
        [ForeignKey("CategoriaMovimientoId")]
        public virtual CategoriaMovimiento CategoriaMovimiento { get; set; } = null!;
        [ForeignKey("TarjetaCreditoId")]
        public virtual TarjetaCredito? TarjetaCredito { get; set; }
    }
}