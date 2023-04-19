namespace SumincogarBackend.DTO.PromocionDTO
{
    public class BuscarPromocion
    {
        public int PromocionId { get; set; }
        public string Titulo { get; set; } = null!;
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaCaducidad { get; set; }
        public bool Prioridad { get; set; }
        public string ImagenPrincipal { get; set; } = null!;
        public List<BuscarPromocionImagen> Imagenes { get; set; } = new List<BuscarPromocionImagen>();
    }
}
