namespace SumincogarBackend.DTO.ProductoDTO
{
    public class BuscarProducto
    {
        public int ProductoId { get; set; }
        public int? FichaTecnicaId { get; set; }
        public string Codigo { get; set; } = null!;
        public string ProductoNombre { get; set; } = null!;
        public string? ImagenUrl { get; set; }
        public List<BuscarImagenRefencial> Imagenes { get; set; } = new List<BuscarImagenRefencial>();
    }
}
