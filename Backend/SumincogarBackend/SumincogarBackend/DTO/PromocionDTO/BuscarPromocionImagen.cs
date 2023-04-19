namespace SumincogarBackend.DTO.PromocionDTO
{
    public class BuscarPromocionImagen
    {
        public int PromocionImagenId { get; set; }
        public int? PromocionId { get; set; }
        public string Url { get; set; } = null!;
        public int Orden { get; set; }
    }
}
