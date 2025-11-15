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
        [Required(ErrorMessage = "El nombre de la tarjeta es obligatorio.")]
        [MaxLength(100)]
        [Display(Name = "Nombre de la Tarjeta")]
        public string Nombre { get; set; } = string.Empty;
        [Required(ErrorMessage = "El límite de crédito es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El límite de crédito debe ser mayor que cero.")]
        [Display(Name = "Límite de Crédito")]
        public decimal? LimiteCredito { get; set; }
        [Required(ErrorMessage = "La deuda actual es obligatoria.")]
        [Range(0.0, double.MaxValue, ErrorMessage = "La deuda actual no puede ser negativa.")]
        [Display(Name = "Deuda Actual")]
        public decimal? DeudaActual { get; set; }
        [Required(ErrorMessage = "El día de corte es obligatorio.")]
        [Range(1, 31, ErrorMessage = "El día de corte debe estar entre 1 y 31.")]
        [Display(Name = "Día de Corte")]
        public int? DiaCorte { get; set; }
        [Required(ErrorMessage = "El día de pago es obligatorio.")]
        [Range(1, 31, ErrorMessage = "El día de pago debe estar entre 1 y 31.")]
        [Display(Name = "Día de Pago")]
        public int? DiaPago { get; set; }
        [Required]
        public bool EsActivo { get; set; } = true;
        // Navigation property
        public virtual ICollection<Movimiento> GastosCredito { get; set; } = new List<Movimiento>();
        public virtual ICollection<PagoTarjeta> Pagos { get; set; } = new List<PagoTarjeta>();
    }
}