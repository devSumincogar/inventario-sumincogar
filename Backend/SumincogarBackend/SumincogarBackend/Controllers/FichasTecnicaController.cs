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
using SumincogarBackend.DTO.CategoriaDTO;
using SumincogarBackend.DTO.FichaTecnicaDTO;
using SumincogarBackend.Models;
using SumincogarBackend.Services.CargarArchivos;

namespace SumincogarBackend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class FichasTecnicaController : ControllerBase
    {
        private readonly db_a977c3_sumincogarContext _context;
        private readonly IMapper _mapper;
        private readonly ICargarArchivos _cargarArchivos;

        public FichasTecnicaController(db_a977c3_sumincogarContext context, IMapper mapper, ICargarArchivos cargarArchivos)
        {
            _context = context;
            _mapper = mapper;
            _cargarArchivos = cargarArchivos;
        }

        [HttpGet("codCliente/{codCliente}")]
        public async Task<ActionResult<IEnumerable<BuscarFichaTecnica>>> GetFichatecnicaXCategoria(string codCliente)
        {
            var fichasTecnicas = await _context.Fichatecnica
                .Include(x => x.Parametrotecnico)
                .Where(x => x.CodCliente!.Equals(codCliente)).ToListAsync();

            return _mapper.Map<List<BuscarFichaTecnica>>(fichasTecnicas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BuscarFichaTecnica>> GetFichatecnica(int id)
        {
            var fichatecnica = await _context.Fichatecnica
                .Include(x => x.Parametrotecnico)
                .FirstOrDefaultAsync(x => x.FichaTecnicaId == id);

            return _mapper.Map<BuscarFichaTecnica>(fichatecnica);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFichatecnica(int id, [FromForm] CrearFichaTecnica crearFichaTecnica)
        {
            var fichaTecnica = await _context.Fichatecnica.FindAsync(id);
            fichaTecnica = _mapper.Map(crearFichaTecnica, fichaTecnica);

            if(crearFichaTecnica.DocumentoUrl != null)
            {
                fichaTecnica!.DocumentoUrl = await _cargarArchivos.ActualizarArchivo(TiposArchivo.FichaTecnica, crearFichaTecnica.DocumentoUrl, fichaTecnica!.DocumentoUrl!);
            }

            try
            {
                _context.Entry(fichaTecnica!).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> PostFichatecnica([FromForm] CrearFichaTecnica crearFichaTecnica)
        {
            var fichaTecnica = _mapper.Map<Fichatecnica>(crearFichaTecnica);

            if (crearFichaTecnica.DocumentoUrl != null)
            {
                fichaTecnica!.DocumentoUrl = await _cargarArchivos.CargarArchivo(TiposArchivo.FichaTecnica, crearFichaTecnica.DocumentoUrl);
            }

            _context.Fichatecnica.Add(fichaTecnica);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFichaTecnica(int id)
        {
            var parametrosTecnicos = await _context.Parametrotecnico.Where(x => x.FichaTecnicaId == id).ToListAsync();

            if(parametrosTecnicos.Count > 0)
            {
                _context.Parametrotecnico.RemoveRange(parametrosTecnicos);
                await _context.SaveChangesAsync();
            }

            var fichaTecnica = await _context.Fichatecnica.FindAsync(id);

            if(fichaTecnica != null)
            {
                _context.Fichatecnica.Remove(fichaTecnica);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
