using SumincogarBackend.Services.AlmacenadorArchivos;

namespace SumincogarBackend.Services.CargarArchivos
{
    public class CargarArchivos : ICargarArchivos
    {
        private readonly IAlmacenadorArchivos _almacenadorArchivos;

        public CargarArchivos(IAlmacenadorArchivos almacenadorArchivos)
        {
            _almacenadorArchivos = almacenadorArchivos;
        }

        public async Task<string> ActualizarArchivo(TiposArchivo tiposArchivo, IFormFile file, string url)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var contenido = memoryStream.ToArray();
                var extension = Path.GetExtension(file.FileName);

                return await _almacenadorArchivos.EditarArchivo(
                     contenido,
                     extension,
                     Contenedor(tiposArchivo),
                     url,
                     file.ContentType
                 );

            }
        }

        public async Task<string> CargarArchivo(TiposArchivo tiposArchivo, IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var contenido = memoryStream.ToArray();
                var extension = Path.GetExtension(file.FileName);

                return await _almacenadorArchivos.GuardarArchivo(
                    contenido,
                    extension,
                    Contenedor(tiposArchivo),
                    file.ContentType
                );

            }
        }

        private string Contenedor(TiposArchivo tiposArchivo)
        {
            switch (tiposArchivo) {
                case TiposArchivo.FichaTecnica:
                    return "fichaTecnica";
                case TiposArchivo.ImagenProducto:
                    return "producto";
                case TiposArchivo.ImagenReferencialProducto:
                    return "imgProductoReferencial";
                default:
                    return "";
            }
        }
    }
}
