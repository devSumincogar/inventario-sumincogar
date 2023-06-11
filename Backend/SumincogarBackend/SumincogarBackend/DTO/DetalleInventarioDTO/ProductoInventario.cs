using System;
using SumincogarBackend.DTO.FichaTecnicaDTO;
using SumincogarBackend.DTO.ProductoDTO;

namespace SumincogarBackend.DTO.DetalleInventarioDTO
{
	public class ProductoInventario
	{
        public int ProductoId { get; set; }        
        public string Codigo { get; set; } = null!;
        public string ProductoNombre { get; set; } = null!;                   
        public string? Stock { get; set; }
        public string? Impresion { get; set; }
        public string? Colores { get; set; }
        public string? Imagen { get; set; }
    }
}

