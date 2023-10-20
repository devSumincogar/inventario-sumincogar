namespace SumincogarBackend.DTO.UsuariosDTO
{
    public class BuscarUsuarioDTO
    {
        public string UsuarioId { get; set; } = null!;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
