using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ControlFinanzasProject.Models
{
    public class TipoMovimiento
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Descripcion { get; set; } = string.Empty;
        [Required]
        public bool EsActivo { get; set; } = true;
        // Navigation property
        public virtual ICollection<CategoriaMovimiento> CategoriaMovimientos { get; set; } = new List<CategoriaMovimiento>();
    }
}