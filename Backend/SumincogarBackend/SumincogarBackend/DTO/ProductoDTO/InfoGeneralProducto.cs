using System;
using SumincogarBackend.DTO.DetalleInventarioDTO;
using SumincogarBackend.DTO.FichaTecnicaDTO;

namespace SumincogarBackend.DTO.ProductoDTO
{
	public class InfoGeneralProducto
	{        
        public string? CodCliente { get; set; }                
        public BuscarFichaTecnica FichaTecnica { get; set; } = new BuscarFichaTecnica();
        public List<BuscarImagenRefencial> Imagenes { get; set; } = new List<BuscarImagenRefencial>();
        public List<ProductoInventario> Productos { get; set; } = new List<ProductoInventario>();
    }
}

