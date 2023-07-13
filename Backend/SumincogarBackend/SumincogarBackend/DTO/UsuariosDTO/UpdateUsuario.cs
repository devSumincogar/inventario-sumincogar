using System.ComponentModel.DataAnnotations;

namespace SumincogarBackend.DTO.UsuariosDTO
{
    public class UpdateUsuario
    {
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
