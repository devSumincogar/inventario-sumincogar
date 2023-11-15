using Microsoft.Build.Framework;

namespace SumincogarBackend.DTO.ImagenReferencialDTO
{
    public class CrearImagenReferencial
    {
        public IFormFile Url { get; set; } = null!;
        [Required]
        public string CodCliente { get; set; } = string.Empty;
    }
}
