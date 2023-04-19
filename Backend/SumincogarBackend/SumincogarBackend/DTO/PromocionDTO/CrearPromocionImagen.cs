using Microsoft.Build.Framework;

namespace SumincogarBackend.DTO.PromocionDTO
{
    public class CrearPromocionImagen
    {
        [Required]
        public int PromocionId { get; set; }
        public IFormFile? Url { get; set; }
        public int Orden { get; set; }
    }
}
