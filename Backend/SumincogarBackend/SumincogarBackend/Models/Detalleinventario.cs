using System;
using System.Collections.Generic;

namespace SumincogarBackend.Models
{
    public partial class Detalleinventario
    {
        public int Detalleinventarioid { get; set; }
        public string? Codcliente { get; set; }
        public string? Codproducto { get; set; }
        public string? Stock { get; set; }
        public string? Impresion { get; set; }
        public string? Colores { get; set; }
    }
}
