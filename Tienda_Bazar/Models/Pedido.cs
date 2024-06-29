using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tienda_Bazar.Models
{
    public class Pedido
    {
        [Key]
        public int CodigoPedido { get; set; }

        [Required]
        public int CodigoUsuario { get; set; }

        public DateTime FechaPedido { get; set; }

        [ForeignKey("CodigoUsuario")]
        public required Usuario Usuario { get; set; }

        // Relaciones
        public ICollection<DetallePedido> DetallesPedidos { get; set; } = new List<DetallePedido>();

    }
}
