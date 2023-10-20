using System.ComponentModel.DataAnnotations;

namespace SumincogarBackend.DTO.PromocionDTO
{
    public class CrearPromocion
    {
        [Required]
        public string Titulo { get; set; } = null!;
        public DateTime FechaCaducidad { get; set; }
        public IFormFile? ImagenPrincipal { get; set; }
    }
}
