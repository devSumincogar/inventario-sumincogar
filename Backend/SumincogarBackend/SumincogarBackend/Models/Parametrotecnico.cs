using System;
using System.Collections.Generic;

namespace SumincogarBackend.Models
{
    public partial class Parametrotecnico
    {
        public int ParametroTecnicoId { get; set; }
        public int? FichaTecnicaId { get; set; }
        public string Clave { get; set; } = null!;
        public string Valor { get; set; } = null!;

        public virtual Fichatecnica? FichaTecnica { get; set; }
    }
}
