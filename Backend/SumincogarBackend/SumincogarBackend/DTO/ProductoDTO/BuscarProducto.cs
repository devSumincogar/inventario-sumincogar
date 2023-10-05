using SumincogarBackend.DTO.GamaColorDTO;

namespace SumincogarBackend.DTO.ProductoDTO
{
    public class BuscarProducto
    {
        public int ProductoId { get; set; }
        public string Codigo { get; set; } = null!;
        public string ProductoNombre { get; set; } = null!;
        public string? ImagenUrl { get; set; }
        public int CategoriaId { get; set; }
        public int? SubcategoriaId { get; set; }
        public List<BuscarGamaColor> GamasColor { get; set; } = new List<BuscarGamaColor>();
        public List<BuscarImagenRefencial> Imagenes { get; set; } = new List<BuscarImagenRefencial>();
    }
}
