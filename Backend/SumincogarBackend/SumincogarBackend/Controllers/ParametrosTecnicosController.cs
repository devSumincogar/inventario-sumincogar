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
using SumincogarBackend.DTO.ParametroTecnicoDTO;
using SumincogarBackend.Models;

namespace SumincogarBackend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ParametrosTecnicosController : ControllerBase
    {
        private readonly db_a977c3_sumincogarContext _context;
        private readonly IMapper _mapper;

        public ParametrosTecnicosController(db_a977c3_sumincogarContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{fichaTecnicaId}")]
        public async Task<ActionResult<IEnumerable<BuscarParametroTecnico>>> GetParametrotecnico(int fichaTecnicaId)
        {
            var parametrosT = await _context.Parametrotecnico.Where(x => x.FichaTecnicaId == fichaTecnicaId).ToListAsync();
            return _mapper.Map<List<BuscarParametroTecnico>>(parametrosT);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BuscarParametroTecnico>> PutParametrotecnico(int id, CrearParametroTecnico crearParametroTecnico)
        {
            var parametroTecnico = await _context.Parametrotecnico.FindAsync(id);
            parametroTecnico = _mapper.Map(crearParametroTecnico, parametroTecnico);

            try
            {
                _context.Entry(parametroTecnico!).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return _mapper.Map<BuscarParametroTecnico>(parametroTecnico);
        }

        [HttpPost]
        public async Task<ActionResult<BuscarParametroTecnico>> PostParametrotecnico(CrearParametroTecnico crearParametroTecnico)
        {
            var parametroTecnico = _mapper.Map<Parametrotecnico>(crearParametroTecnico);

            _context.Parametrotecnico.Add(parametroTecnico);
            await _context.SaveChangesAsync();

            return _mapper.Map<BuscarParametroTecnico>(parametroTecnico);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParametrotecnico(int id)
        {
            if (_context.Parametrotecnico == null)
            {
                return NotFound();
            }
            var parametrotecnico = await _context.Parametrotecnico.FindAsync(id);
            if (parametrotecnico == null)
            {
                return NotFound();
            }

            _context.Parametrotecnico.Remove(parametrotecnico);
            await _context.SaveChangesAsync();

            return Ok();
        }        
    }
}
