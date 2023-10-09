using System.ComponentModel.DataAnnotations;

namespace SumincogarBackend.DTO.ProductoDTO
{
    public class CrearProducto
    {        
        public string Codigo { get; set; } = null!;
        public string ProductoNombre { get; set; } = null!;
        public int? SubcategoriaId { get; set; }
        public IFormFile? ImagenUrl { get; set; }
    }
}
