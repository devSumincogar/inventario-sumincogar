using System;
using System.Collections.Generic;

namespace SumincogarBackend.Models
{
    public partial class Categorium
    {
        public Categorium()
        {
            Productos = new HashSet<Producto>();
        }

        public int Categoriaid { get; set; }
        public string Categorianombre { get; set; } = null!;

        public virtual ICollection<Producto> Productos { get; set; }
    }
}
