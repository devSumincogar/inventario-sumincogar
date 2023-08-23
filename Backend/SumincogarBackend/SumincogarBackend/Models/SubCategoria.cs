using System;
using System.Collections.Generic;

namespace SumincogarBackend.Models
{
    public partial class SubCategoria
    {
        public SubCategoria()
        {
            Fichatecnica = new HashSet<Fichatecnica>();
            Producto = new HashSet<Producto>();
        }

        public int SubcategoriaId { get; set; }
        public string? SubcategoriaNombre { get; set; }
        public int CategoriaId { get; set; }

        public virtual Categoria Categoria { get; set; } = null!;
        public virtual ICollection<Fichatecnica> Fichatecnica { get; set; }
        public virtual ICollection<Producto> Producto { get; set; }
    }
}
