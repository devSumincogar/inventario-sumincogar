using System;
using System.Collections.Generic;

namespace SumincogarBackend.Models
{
    public partial class Imagenreferencial
    {
        public int Imagenreferencialid { get; set; }
        public int? Productoid { get; set; }
        public string? Imagenreferencialurl { get; set; }

        public virtual Producto? Producto { get; set; }
    }
}
