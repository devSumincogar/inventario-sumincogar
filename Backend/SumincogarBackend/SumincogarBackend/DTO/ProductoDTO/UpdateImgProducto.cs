using Microsoft.Build.Framework;

namespace SumincogarBackend.DTO.ProductoDTO
{
    public class UpdateImgProducto
    {
        [Required]
        public string Codigo { get; set; } = null!;
        [Required]
        public IFormFile? ImagenUrl { get; set; }
    }
}
