namespace SumincogarBackend.DTO.ParametroTecnicoDTO
{
    public class BuscarParametroTecnico
    {
        public int ParametroTecnicoId { get; set; }
        public int? FichaTecnicaId { get; set; }
        public string Clave { get; set; } = null!;
        public string Valor { get; set; } = null!;
    }
}
