using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tienda_Bazar.Models
{
    public class ImagenProducto
    {
        [Key]
        public int CodigoImagen { get; set; }

        [Required]
        public int CodigoProducto { get; set; }

        [Required]
        public required byte[] Imagen { get; set; }

        [ForeignKey("CodigoProducto")]
        public required Producto Producto { get; set; }
    }
}
