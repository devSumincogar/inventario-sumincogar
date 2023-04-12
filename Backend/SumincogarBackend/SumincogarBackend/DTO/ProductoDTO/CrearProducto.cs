using System.ComponentModel.DataAnnotations;

namespace SumincogarBackend.DTO.ProductoDTO
{
    public class CrearProducto
    {
        [Required]
        public int Categoriaid { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Productonombre { get; set; } = string.Empty!;
        public IFormFile? Fichatenicapdf { get; set; }
        public IFormFile? Imagenreferencial { get; set; }
    }
}
