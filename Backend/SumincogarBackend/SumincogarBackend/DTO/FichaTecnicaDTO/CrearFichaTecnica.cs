using Microsoft.Build.Framework;

namespace SumincogarBackend.DTO.FichaTecnicaDTO
{
    public class CrearFichaTecnica
    {
        [Required]
        public int CategoriaId { get; set; }
        public string NombreFichaTecnica { get; set; } = string.Empty;
        public IFormFile? DocumentoUrl { get; set; }
    }
}
