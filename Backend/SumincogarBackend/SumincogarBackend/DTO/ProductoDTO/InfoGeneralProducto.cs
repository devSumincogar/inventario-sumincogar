using System;
using SumincogarBackend.DTO.DetalleInventarioDTO;
using SumincogarBackend.DTO.FichaTecnicaDTO;
using SumincogarBackend.DTO.ImagenReferencialDTO;

namespace SumincogarBackend.DTO.ProductoDTO
{
	public class InfoGeneralProducto
	{        
        public string? CodCliente { get; set; }            
        public bool Descontinuada { get; set; }
        public string TelasSimilares { get; set; } = string.Empty;
        public BuscarFichaTecnica FichaTecnica { get; set; } = new BuscarFichaTecnica();
        public List<Imagen> Imagenes { get; set; } = new List<Imagen>();
        public List<ProductoInventario> Productos { get; set; } = new List<ProductoInventario>();
    }
}

