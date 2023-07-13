using System;
using System.Collections.Generic;

namespace SumincogarBackend.Models
{
    public partial class Detalleinventario
    {
        public int DetalleInventarioId { get; set; }
        public string? CodCliente { get; set; }
        public string? CodProducto { get; set; }
        public string? Stock { get; set; }
        public string? Impresion { get; set; }
        public string? Colores { get; set; }
        public bool? Descontinuada { get; set; }
        public string TelasSimilares { get; set; } = null!;
    }
}
