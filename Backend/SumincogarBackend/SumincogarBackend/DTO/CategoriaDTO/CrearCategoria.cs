using System.ComponentModel.DataAnnotations;

namespace SumincogarBackend.DTO.CategoriaDTO
{
    public class CrearCategoria
    {
        [Required]
        public string Categorianombre { get; set; } = string.Empty;
    }
}
