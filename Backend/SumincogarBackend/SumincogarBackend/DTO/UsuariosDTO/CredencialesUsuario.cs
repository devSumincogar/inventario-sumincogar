using System.ComponentModel.DataAnnotations;

namespace SumincogarBackend.DTO.UsuariosDTO
{
    public class CredencialesUsuario
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
