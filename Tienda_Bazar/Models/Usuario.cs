using System.ComponentModel.DataAnnotations;

namespace Tienda_Bazar.Models
{
    public class Usuario
    {
        [Key]
        public int CodigoUsuario { get; set; }

        [Required]
        [MaxLength(100)]
        public required string NombreUsuario { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Contrasena { get; set; }

        public DateTime UltimaConexion { get; set; }

        public bool Estado { get; set; }

        // Relaciones
        public ICollection<CarritoCompra> CarritoCompras { get; set; } = new List<CarritoCompra>();
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public ICollection<Resena> Resenas { get; set; } = new List<Resena>();
    }
}
