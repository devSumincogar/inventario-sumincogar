using SumincogarBackend.DTO.GamaColorDTO;
using SumincogarBackend.DTO.ImagenReferencialDTO;

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
        public List<Imagen> Imagenes { get; set; } = new List<Imagen>();
    }
}
