using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tienda_Bazar.Models
{
    public class CarritoCompra
    {
        [Key]
        public int CodigoCarrito { get; set; }

        [Required]
        public int CodigoUsuario { get; set; }

        [Required]
        public int CodigoProducto { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [ForeignKey("CodigoUsuario")]
        public required Usuario Usuario { get; set; }

        [ForeignKey("CodigoProducto")]
        public required Producto Producto { get; set; }

    }
}
