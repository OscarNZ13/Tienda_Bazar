using System.ComponentModel.DataAnnotations;

namespace Tienda_Bazar.Models
{
    public class EstadoUsuario
    {
        [Key]
        public int EstadoUsuarioId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = null!;

        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
