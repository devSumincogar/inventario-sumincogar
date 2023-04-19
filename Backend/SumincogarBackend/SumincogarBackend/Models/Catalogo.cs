using System;
using System.Collections.Generic;

namespace SumincogarBackend.Models
{
    public partial class Catalogo
    {
        public int CatalogoId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string ImagenUrl { get; set; } = null!;
    }
}
