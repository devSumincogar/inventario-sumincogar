using System.ComponentModel.DataAnnotations;

namespace SumincogarBackend.DTO.PromocionDTO
{
    public class CrearPromocion
    {
        [Required]
        public string Titulo { get; set; } = null!;
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaCaducidad { get; set; }
        public bool Prioridad { get; set; }
        public IFormFile? ImagenPrincipal { get; set; }
    }
}
