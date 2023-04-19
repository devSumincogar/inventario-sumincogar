using System;
using System.Collections.Generic;

namespace SumincogarBackend.Models
{
    public partial class Promocion
    {
        public Promocion()
        {
            Promocionimagen = new HashSet<Promocionimagen>();
        }

        public int PromocionId { get; set; }
        public string Titulo { get; set; } = null!;
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaCaducidad { get; set; }
        public bool Prioridad { get; set; }
        public string ImagenPrincipal { get; set; } = null!;

        public virtual ICollection<Promocionimagen> Promocionimagen { get; set; }
    }
}
