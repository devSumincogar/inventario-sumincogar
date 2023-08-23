using System;
using System.Collections.Generic;

namespace SumincogarBackend.Models
{
    public partial class Usuario
    {
        public string UsuarioId { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public bool Tutorial { get; set; }
        public bool ResetPassword { get; set; }
    }
}
