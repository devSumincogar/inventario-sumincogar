using System;
using System.Collections.Generic;

namespace SumincogarBackend.Models
{
    public partial class Promocion
    {
        public Promocion()
        {
            Promocionimagens = new HashSet<Promocionimagen>();
        }

        public int Promocionid { get; set; }
        public string Titulo { get; set; } = null!;
        public DateTime Fechaingreso { get; set; }
        public DateTime Fechacaducidad { get; set; }
        public bool Prioridad { get; set; }
        public string Imagenprincipal { get; set; } = null!;

        public virtual ICollection<Promocionimagen> Promocionimagens { get; set; }
    }
}
