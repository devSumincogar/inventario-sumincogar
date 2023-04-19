using System;
using System.Collections.Generic;

namespace SumincogarBackend.Models
{
    public partial class Imagenreferencial
    {
        public int ImagenReferenciaId { get; set; }
        public int? ProductoId { get; set; }
        public string? Url { get; set; }

        public virtual Producto? Producto { get; set; }
    }
}
