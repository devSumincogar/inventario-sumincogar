using System;
using System.Collections.Generic;

namespace SumincogarBackend.Models
{
    public partial class Producto
    {
        public Producto()
        {
            Imagenreferencials = new HashSet<Imagenreferencial>();
            Parametrotecnicos = new HashSet<Parametrotecnico>();
        }

        public int Productoid { get; set; }
        public int? Categoriaid { get; set; }
        public string Codigo { get; set; } = null!;
        public string Productonombre { get; set; } = null!;
        public string? Fichatenicapdf { get; set; }
        public string? Imagenreferencial { get; set; }

        public virtual Categorium? Categoria { get; set; }
        public virtual ICollection<Imagenreferencial> Imagenreferencials { get; set; }
        public virtual ICollection<Parametrotecnico> Parametrotecnicos { get; set; }
    }
}
