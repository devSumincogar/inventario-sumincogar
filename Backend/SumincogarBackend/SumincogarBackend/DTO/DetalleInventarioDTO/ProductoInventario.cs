using System;
using SumincogarBackend.DTO.FichaTecnicaDTO;
using SumincogarBackend.DTO.ProductoDTO;

namespace SumincogarBackend.DTO.DetalleInventarioDTO
{
	public class ProductoInventario
	{     
        public int SubCategoriaId { get; set; }
        public string SubCategoriaNombre { get; set; } = string.Empty;
        public string Codigo { get; set; } = null!;
        public string CodCliente { get; set; } = null!;
        public string ProductoNombre { get; set; } = null!;                   
        public string? Stock { get; set; }
        public int Orden { get; set; }
        public string? Impresion { get; set; }
        public string? Colores { get; set; }
        public string? Imagen { get; set; }
    }
}

