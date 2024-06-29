using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tienda_Bazar.Models
{
    public class DetallePedido
    {
        [Key]
        public int CodigoDetalle { get; set; }

        [Required]
        public int CodigoPedido { get; set; }

        [Required]
        public int CodigoProducto { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioUnitario { get; set; }

        [ForeignKey("CodigoPedido")]
        public required Pedido Pedido { get; set; }

        [ForeignKey("CodigoProducto")]
        public required Producto Producto { get; set; }

    }
}
