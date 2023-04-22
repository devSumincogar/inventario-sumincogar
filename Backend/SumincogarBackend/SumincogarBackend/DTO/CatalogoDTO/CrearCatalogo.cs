namespace SumincogarBackend.DTO.CatalogoDTO
{
    public class CrearCatalogo
    {
        public string Nombre { get; set; } = null!;
        public IFormFile? Url { get; set; } = null!;
        public IFormFile? ImagenUrl { get; set; } = null!;
    }
}
