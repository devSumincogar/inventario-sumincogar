using Microsoft.Build.Framework;

namespace SumincogarBackend.DTO.ParametroTecnicoDTO
{
    public class CrearParametroTecnico
    {
        [Required]
        public int? FichaTecnicaId { get; set; }
        public string Clave { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
    }
}
