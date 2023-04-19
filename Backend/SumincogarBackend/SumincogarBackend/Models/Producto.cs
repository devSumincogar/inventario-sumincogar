using System;
using System.Collections.Generic;

namespace SumincogarBackend.Models
{
    public partial class Producto
    {
        public Producto()
        {
            Imagenreferencial = new HashSet<Imagenreferencial>();
        }

        public int ProductoId { get; set; }
        public int? FichaTecnicaId { get; set; }
        public string Codigo { get; set; } = null!;
        public string ProductoNombre { get; set; } = null!;
        public string? ImagenUrl { get; set; }

        public virtual Fichatecnica? FichaTecnica { get; set; }
        public virtual ICollection<Imagenreferencial> Imagenreferencial { get; set; }
    }
}
