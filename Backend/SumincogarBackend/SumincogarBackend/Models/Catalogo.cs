using System;
using System.Collections.Generic;

namespace SumincogarBackend.Models
{
    public partial class Catalogo
    {
        public int Catalogoid { get; set; }
        public string Nombre { get; set; } = null!;
        public string Catalogourl { get; set; } = null!;
        public string Imagenreferencial { get; set; } = null!;
    }
}
