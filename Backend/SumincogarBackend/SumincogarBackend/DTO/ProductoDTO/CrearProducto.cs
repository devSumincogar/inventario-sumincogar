using System.ComponentModel.DataAnnotations;

namespace SumincogarBackend.DTO.ProductoDTO
{
    public class CrearProducto
    {
        public int? FichaTecnicaId { get; set; }
        public string Codigo { get; set; } = null!;
        public string ProductoNombre { get; set; } = null!;
        public IFormFile? ImagenUrl { get; set; }
    }
}
