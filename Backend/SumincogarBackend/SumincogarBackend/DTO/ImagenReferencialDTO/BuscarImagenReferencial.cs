namespace SumincogarBackend.DTO.ImagenReferencialDTO
{
    public class BuscarImagenReferencial
    {
        
        public string? CodCliente { get; set; }
        public List<Imagen> Imagenes { get; set; } = new List<Imagen>();
    }

    public class Imagen
    {
        public int ImagenReferenciaId { get; set; }
        public string? Url { get; set; }
    }
}
