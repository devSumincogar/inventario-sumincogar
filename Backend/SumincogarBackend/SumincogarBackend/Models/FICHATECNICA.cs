using System;
using System.Collections.Generic;

namespace SumincogarBackend.Models
{
    public partial class Fichatecnica
    {
        public Fichatecnica()
        {
            Parametrotecnico = new HashSet<Parametrotecnico>();
            Producto = new HashSet<Producto>();
        }

        public int FichaTecnicaId { get; set; }
        public int? CategoriaId { get; set; }
        public string? DocumentoUrl { get; set; }
        public string NombreFichaTecnica { get; set; } = null!;

        public virtual Categoria? Categoria { get; set; }
        public virtual ICollection<Parametrotecnico> Parametrotecnico { get; set; }
        public virtual ICollection<Producto> Producto { get; set; }
    }
}
