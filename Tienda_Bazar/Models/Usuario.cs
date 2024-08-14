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

        [MaxLength(255)]
        public string? Contrasena { get; set; }

        public DateTime UltimaConexion { get; set; }

        // Relaciones
        public int EstadoUsuarioId { get; set; }
        public EstadoUsuario EstadoUsuario { get; set; } = null!;

        public int RolId { get; set; }
        public Rol Rol { get; set; } = null!;

        // Relaciones
        public ICollection<CarritoCompra> CarritoCompras { get; set; } = new List<CarritoCompra>();
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public ICollection<Resena> Resenas { get; set; } = new List<Resena>();
    }
}
