using Microsoft.Build.Framework;

namespace SumincogarBackend.DTO.FichaTecnicaDTO
{
    public class CrearFichaTecnica
    {
        public string CodCliente { get; set; } = string.Empty;
        public string NombreFichaTecnica { get; set; } = string.Empty;
        public IFormFile? DocumentoUrl { get; set; }
    }
}
