using System.ComponentModel.DataAnnotations;

namespace SumincogarBackend.DTO.UsuariosDTO
{
    public class UpdateUsuario
    {
        [Required]
        public string UsuarioId { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string? Email { get; set; }
    }
}
