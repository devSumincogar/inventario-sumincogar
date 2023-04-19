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
using SumincogarBackend.DTO.CatalogoDTO;
using SumincogarBackend.DTO.FichaTecnicaDTO;
using SumincogarBackend.Models;
using SumincogarBackend.Services.CargarArchivos;

namespace SumincogarBackend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class CatalogosController : ControllerBase
    {
        private readonly db_a977c3_sumincogarContext _context;
        private readonly IMapper _mapper;
        private readonly ICargarArchivos _cargarArchivos;

        public CatalogosController(db_a977c3_sumincogarContext context, IMapper mapper, ICargarArchivos cargarArchivos)
        {
            _context = context;
            _mapper = mapper;
            _cargarArchivos = cargarArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BuscarCatalogo>>> GetCatalogo([FromQuery] string nombre)
        {
            var catalogos = await _context.Catalogo.Where(x => x.Nombre.Contains(nombre)).ToListAsync();
            return _mapper.Map<List<BuscarCatalogo>>(catalogos);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BuscarCatalogo>> PutCatalogo(int id, CrearCatalogo crearCatalogo)
        {
            var catalogo = await _context.Catalogo.FindAsync(id);
            catalogo = _mapper.Map(crearCatalogo, catalogo);

            if (crearCatalogo.Url != null)
            {
                catalogo!.Url = await _cargarArchivos.ActualizarArchivo(TiposArchivo.Catalogo, crearCatalogo.Url, catalogo!.Url!);
            }

            if (crearCatalogo.ImagenUrl != null)
            {
                catalogo!.ImagenUrl = await _cargarArchivos.ActualizarArchivo(TiposArchivo.Catalogo, crearCatalogo.ImagenUrl, catalogo!.ImagenUrl!);
            }

            try
            {
                _context.Entry(catalogo!).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return _mapper.Map<BuscarCatalogo>(catalogo);
        }

        [HttpPost]
        public async Task<ActionResult<BuscarCatalogo>> PostCatalogo(CrearCatalogo crearCatalogo)
        {
            var catalogo = _mapper.Map<Catalogo>(crearCatalogo);

            if (crearCatalogo.Url != null)
            {
                catalogo!.Url = await _cargarArchivos.CargarArchivo(TiposArchivo.Catalogo, crearCatalogo.Url);
            }

            if (crearCatalogo.ImagenUrl != null)
            {
                catalogo!.ImagenUrl = await _cargarArchivos.CargarArchivo(TiposArchivo.Catalogo, crearCatalogo.ImagenUrl);
            }

            _context.Catalogo.Add(catalogo);
            await _context.SaveChangesAsync();

            return _mapper.Map<BuscarCatalogo>(catalogo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCatalogo(int id)
        {
            if (_context.Catalogo == null)
            {
                return NotFound();
            }
            var catalogo = await _context.Catalogo.FindAsync(id);
            if (catalogo == null)
            {
                return NotFound();
            }

            _context.Catalogo.Remove(catalogo);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
