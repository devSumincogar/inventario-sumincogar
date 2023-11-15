using System;
using System.Collections.Generic;

namespace SumincogarBackend.Models
{
    public partial class Fichatecnica
    {
        public Fichatecnica()
        {
            Parametrotecnico = new HashSet<Parametrotecnico>();
        }

        public int FichaTecnicaId { get; set; }
        public string? DocumentoUrl { get; set; }
        public string NombreFichaTecnica { get; set; } = null!;
        public string? CodCliente { get; set; }

        public virtual ICollection<Parametrotecnico> Parametrotecnico { get; set; }
    }
}
