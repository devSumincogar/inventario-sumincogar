using System.ComponentModel.DataAnnotations;

namespace SumincogarBackend.DTO.UsuariosDTO
{
    public class EmailUsuario
    {
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
