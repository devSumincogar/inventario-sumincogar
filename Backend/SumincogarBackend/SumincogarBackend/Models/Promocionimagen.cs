using System;
using System.Collections.Generic;

namespace SumincogarBackend.Models
{
    public partial class Promocionimagen
    {
        public int Promocionimagenid { get; set; }
        public int? Promocionid { get; set; }
        public string Promocionimagenurl { get; set; } = null!;
        public int Orden { get; set; }

        public virtual Promocion? Promocion { get; set; }
    }
}
