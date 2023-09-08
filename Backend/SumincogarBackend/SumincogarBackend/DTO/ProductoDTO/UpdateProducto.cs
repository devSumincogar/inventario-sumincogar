using System;
namespace SumincogarBackend.DTO.ProductoDTO
{
	public class UpdateProducto
	{
        public int ProductoId { get; set; }
        public string Codigo { get; set; } = null!;
        public string ProductoNombre { get; set; } = string.Empty;
        public int SubcategoriaId { get; set; }
    }
}

