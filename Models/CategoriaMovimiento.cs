using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ControlFinanzasProject.Models
{
    public class CategoriaMovimiento
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Descripcion { get; set; } = string.Empty;
        [Required]
        public bool EsActivo { get; set; } = true;
        // foreign key
        [Required]
        public int TipoMovimientoId { get; set; }
        // Navigation property
        [ForeignKey("TipoMovimientoId")]
        public virtual TipoMovimiento TipoMovimiento { get; set; } = null!;
        // Navigation property
        public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
    }
}