using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SumincogarBackend.Contexts;
using SumincogarBackend.DTO.ImagenReferencialDTO;
using SumincogarBackend.Models;
using SumincogarBackend.Services.CargarArchivos;

namespace SumincogarBackend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ImagenReferencialController : ControllerBase
    {
        private readonly db_a977c3_sumincogarContext _context;
        private readonly IMapper _mapper;
        private readonly ICargarArchivos _cargarArchivos;

        public ImagenReferencialController(db_a977c3_sumincogarContext context, IMapper mapper, ICargarArchivos cargarArchivos)
        {
            _context = context;
            _mapper = mapper;
            _cargarArchivos = cargarArchivos;
        }

        [HttpGet("{codCliente}")]
        public async Task<ActionResult<BuscarImagenReferencial>> GetImagenesReferenciales(string codCliente)
        {
            var imagenes = await _context.Imagenreferencial.Where(x => x.CodCliente!.Equals(codCliente))
                .Select(x => new Imagen {
                    ImagenReferenciaId = x.ImagenReferenciaId,
                    Url = x.Url
                })
                .ToListAsync();

            var imagenesReferenciales = new BuscarImagenReferencial { CodCliente = codCliente, Imagenes = imagenes };

            return imagenesReferenciales;
        }

        [HttpPut]
        public async Task<ActionResult<BuscarImagenReferencial>> PostImagenReferencial(ActualizarImagenReferencial actualizarImagenReferencial)
        {
            var imagenesReferenciales = await _context.Imagenreferencial
                .Where(x => x.CodCliente!.Equals(actualizarImagenReferencial.CodClienteAnterior))
                .ToListAsync();

            if (imagenesReferenciales.Count == 0) return BadRequest();

            imagenesReferenciales.ForEach(x => x.CodCliente = actualizarImagenReferencial.CodCliente);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpPost]
        public async Task<ActionResult<BuscarImagenReferencial>> PostImagenReferencial([FromForm] CrearImagenReferencial crearImagenReferencial)
        {
            var imagenReferencial = _mapper.Map<Imagenreferencial>(crearImagenReferencial);

            if (crearImagenReferencial.Url != null)
            {
                imagenReferencial!.Url = await _cargarArchivos.CargarArchivo(TiposArchivo.ImagenProducto, crearImagenReferencial.Url);
            }

            _context.Imagenreferencial.Add(imagenReferencial);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategorium(int id)
        {
            var imagenReferencial = await _context.Imagenreferencial.FindAsync(id);

            _context.Imagenreferencial.Remove(imagenReferencial!);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
