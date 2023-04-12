using System;
using System.Collections.Generic;

namespace SumincogarBackend.Models
{
    public partial class Parametrotecnico
    {
        public int Parametrotecnicoid { get; set; }
        public int? Productoid { get; set; }
        public string Clave { get; set; } = null!;
        public string Valor { get; set; } = null!;

        public virtual Producto? Producto { get; set; }
    }
}
