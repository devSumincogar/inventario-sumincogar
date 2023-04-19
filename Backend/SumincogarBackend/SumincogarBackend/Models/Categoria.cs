using System;
using System.Collections.Generic;

namespace SumincogarBackend.Models
{
    public partial class Categoria
    {
        public Categoria()
        {
            Fichatecnica = new HashSet<Fichatecnica>();
        }

        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; } = null!;

        public virtual ICollection<Fichatecnica> Fichatecnica { get; set; }
    }
}
