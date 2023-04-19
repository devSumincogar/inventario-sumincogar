using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SumincogarBackend.Contexts;
using SumincogarBackend.DTO.FichaTecnicaDTO;
using SumincogarBackend.DTO.PromocionDTO;
using SumincogarBackend.Models;
using SumincogarBackend.Services.CargarArchivos;

namespace SumincogarBackend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PromocionesController : ControllerBase
    {
        private readonly db_a977c3_sumincogarContext _context;
        private readonly IMapper _mapper;
        private readonly ICargarArchivos _cargarArchivos;

        public PromocionesController(db_a977c3_sumincogarContext context, IMapper mapper, ICargarArchivos cargarArchivos)
        {
            _context = context;
            _mapper = mapper;
            _cargarArchivos = cargarArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BuscarPromocion>>> GetPromocion()
        {
            var promociones = await _context.Promocion.Include(x => x.Promocionimagen)
                .OrderBy(x => x.FechaIngreso)
                .ToListAsync();
            return _mapper.Map<List<BuscarPromocion>>(promociones);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BuscarPromocion>>> GetPromocionesDisponibles()
        {
            var promociones = await _context.Promocion.Include(x => x.Promocionimagen)
                .Where(x => DateTime.Now > x.FechaCaducidad)
                .OrderBy(x => x.FechaIngreso)
                .ToListAsync();

            return _mapper.Map<List<BuscarPromocion>>(promociones);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BuscarPromocion>> PutPromocion(int id, CrearPromocion crearPromocion)
        {
            var promocion = await _context.Promocion.FindAsync(id);
            promocion = _mapper.Map(crearPromocion, promocion);

            if (crearPromocion.ImagenPrincipal != null)
            {
                promocion!.ImagenPrincipal = await _cargarArchivos.ActualizarArchivo(TiposArchivo.Promocion, crearPromocion.ImagenPrincipal, promocion!.ImagenPrincipal!);
            }

            try
            {
                _context.Entry(promocion!).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            promocion = await _context.Promocion.Include(x => x.Promocionimagen)
                .FirstOrDefaultAsync(x => x.PromocionId == id);

            return _mapper.Map<BuscarPromocion>(promocion);
        }

        [HttpPost]
        public async Task<ActionResult<BuscarPromocion>> PostPromocion(CrearPromocion crearPromocion)
        {
            var promocion = _mapper.Map<Promocion>(crearPromocion);

            if (crearPromocion.ImagenPrincipal != null)
            {
                promocion!.ImagenPrincipal = await _cargarArchivos.CargarArchivo(TiposArchivo.Promocion, crearPromocion.ImagenPrincipal);
            }

            _context.Promocion.Add(promocion);
            await _context.SaveChangesAsync();

            promocion = await _context.Promocion.Include(x => x.Promocionimagen)
                .FirstOrDefaultAsync(x => x.PromocionId == promocion.PromocionId);

            return _mapper.Map<BuscarPromocion>(promocion);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePromocion(int id)
        {
            if (_context.Promocion == null)
            {
                return NotFound();
            }
            var promocion = await _context.Promocion.FindAsync(id);
            if (promocion == null)
            {
                return NotFound();
            }

            _context.Promocion.Remove(promocion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("promocionImagen")]
        public async Task<IActionResult> PostPromocionImagen(CrearPromocionImagen crearPromocionImagen)
        {
            var promocionImagen = _mapper.Map<Promocionimagen>(crearPromocionImagen);

            if (crearPromocionImagen.Url != null)
            {
                promocionImagen!.Url = await _cargarArchivos.CargarArchivo(TiposArchivo.Promocion, crearPromocionImagen.Url);
            }

            try
            {
                _context.Promocionimagen.Add(promocionImagen);
                await _context.SaveChangesAsync();
            }catch (Exception)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("promocionImagen/{id}")]
        public async Task<IActionResult> DeletePromocionImagen(int id)
        {
            if (_context.Promocionimagen == null)
            {
                return NotFound();
            }
            var promocion = await _context.Promocionimagen.FindAsync(id);
            if (promocion == null)
            {
                return NotFound();
            }

            _context.Promocionimagen.Remove(promocion);
            await _context.SaveChangesAsync();

            return NoContent();
        }        
    }
}
