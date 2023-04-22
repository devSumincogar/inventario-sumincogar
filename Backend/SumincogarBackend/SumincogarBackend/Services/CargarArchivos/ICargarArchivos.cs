namespace SumincogarBackend.Services.CargarArchivos
{
    public interface ICargarArchivos
    {
        public Task<string> CargarArchivo(TiposArchivo tiposArchivo, IFormFile file);
        public Task<string> ActualizarArchivo(TiposArchivo tiposArchivo, IFormFile file, string url);
        public Task BorrarArchivo(TiposArchivo tiposArchivo, string ruta);
    }
}
