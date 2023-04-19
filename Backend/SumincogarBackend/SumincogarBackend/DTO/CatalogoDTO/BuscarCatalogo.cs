namespace SumincogarBackend.DTO.CatalogoDTO
{
    public class BuscarCatalogo
    {
        public int CatalogoId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string ImagenUrl { get; set; } = null!;
    }
}
