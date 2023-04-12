namespace SumincogarBackend.DTO.UsuariosDTO
{
    public class RespuestaLogin
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiracion { get; set; }

    }
}
