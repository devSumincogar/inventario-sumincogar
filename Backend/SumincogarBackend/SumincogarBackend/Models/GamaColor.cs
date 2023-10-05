using System;
using System.Collections.Generic;

namespace SumincogarBackend.Models
{
    public partial class GamaColor
    {
        public GamaColor()
        {
            ProductoGamacolor = new HashSet<ProductoGamacolor>();
        }

        public int GamaColorId { get; set; }
        public string? GamaColorNombre { get; set; }

        public virtual ICollection<ProductoGamacolor> ProductoGamacolor { get; set; }
    }
}
