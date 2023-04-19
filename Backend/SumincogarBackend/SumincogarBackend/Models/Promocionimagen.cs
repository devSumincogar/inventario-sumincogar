using System;
using System.Collections.Generic;

namespace SumincogarBackend.Models
{
    public partial class Promocionimagen
    {
        public int PromocionImagenId { get; set; }
        public int? PromocionId { get; set; }
        public string Url { get; set; } = null!;
        public int Orden { get; set; }

        public virtual Promocion? Promocion { get; set; }
    }
}
