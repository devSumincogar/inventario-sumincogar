using System;
using System.Collections.Generic;

namespace SumincogarBackend.Models
{
    public partial class ProductoGamacolor
    {
        public int ProductoId { get; set; }
        public int GamaColorId { get; set; }
        public int ProductoGamacolorId { get; set; }

        public virtual GamaColor GamaColor { get; set; } = null!;
        public virtual Producto Producto { get; set; } = null!;
    }
}
