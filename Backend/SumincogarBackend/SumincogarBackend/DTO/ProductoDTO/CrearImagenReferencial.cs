using Microsoft.Build.Framework;

namespace SumincogarBackend.DTO.ProductoDTO
{
    public class CrearImagenReferencial
    {
        [Required]
        public int? ProductoId { get; set; }
        public IFormFile? Url { get; set; }
    }
}
