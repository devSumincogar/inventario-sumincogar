using System.ComponentModel.DataAnnotations;

namespace SumincogarBackend.DTO.UsuariosDTO
{
    public class CrearUsuarioDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        public string Apellido { get; set; } = string.Empty;
    }
}
