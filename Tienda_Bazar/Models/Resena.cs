using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tienda_Bazar.Models
{
    public class Resena
    {
        [Key]
        public int CodigoResena { get; set; }

        [Required]
        public int CodigoProducto { get; set; }

        [Required]
        public int CodigoUsuario { get; set; }

        [Required]
        [MaxLength(1000)]
        public required string DescipcionResena { get; set; }

        public DateTime FechaResena { get; set; }

        [ForeignKey("CodigoProducto")]
        public required Producto Producto { get; set; }

        [ForeignKey("CodigoUsuario")]
        public required Usuario Usuario { get; set; }
    }
}
