using SumincogarBackend.DTO.ParametroTecnicoDTO;

namespace SumincogarBackend.DTO.FichaTecnicaDTO
{
    public class BuscarFichaTecnica
    {
        public int FichaTecnicaId { get; set; }
        public int SubCategoriaId { get; set; }
        public string SubCategoriaName { get; set; } = string.Empty;
        public string NombreFichaTecnica { get; set; } = string.Empty;
        public string DocumentoUrl { get; set; } = string.Empty;
        public List<BuscarParametroTecnico> Parametros { get; set; } = new List<BuscarParametroTecnico>();
    }
}
