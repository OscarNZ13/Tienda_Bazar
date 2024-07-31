using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tienda_Bazar.Models
{
    public class Producto
    {
        [Key] public int CodigoProducto { get; set; }

        [Required]
        [MaxLength(100)] public required string NombreProducto { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")] public decimal Precio { get; set; }

        [Required]
        public int DisponibilidadInventario { get; set; }

        // Esta propiedad debe tener un getter y un setter públicos
        public bool Estado { get; set; }

        // Relaciones
        public ICollection<ImagenProducto> ImagenesProductos { get; set; } = new List<ImagenProducto>();
        public ICollection<CarritoCompra> CarritoCompras { get; set; } = new List<CarritoCompra>();
        public ICollection<DetallePedido> DetallesPedidos { get; set; } = new List<DetallePedido>();
        public ICollection<Resena> Resenas { get; set; } = new List<Resena>();

    }
}
