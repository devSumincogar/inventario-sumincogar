using Microsoft.Build.Framework;

namespace SumincogarBackend.DTO.SubCategoriaDTO
{
    public class CrearSubCategoria
    {
        [Required]
        public string SubcategoriaNombre { get; set; } = string.Empty;
        [Required]
        public int CategoriaId { get; set; }
    }
}
