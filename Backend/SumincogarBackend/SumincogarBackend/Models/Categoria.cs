using System;
using System.Collections.Generic;

namespace SumincogarBackend.Models
{
    public partial class Categoria
    {
        public Categoria()
        {
            Fichatecnica = new HashSet<Fichatecnica>();
            SubCategoria = new HashSet<SubCategoria>();
        }

        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; } = null!;

        public virtual ICollection<Fichatecnica> Fichatecnica { get; set; }
        public virtual ICollection<SubCategoria> SubCategoria { get; set; }
    }
}
