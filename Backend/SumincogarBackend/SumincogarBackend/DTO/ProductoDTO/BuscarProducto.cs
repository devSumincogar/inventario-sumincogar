namespace SumincogarBackend.DTO.ProductoDTO
{
    public class BuscarProducto
    {
        public int Productoid { get; set; }
        public int Categoriaid { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Productonombre { get; set; } = string.Empty;
        public string Fichatenicapdf { get; set; } = string.Empty;
        public string Imagenreferencial { get; set; } = string.Empty;
    }
}
