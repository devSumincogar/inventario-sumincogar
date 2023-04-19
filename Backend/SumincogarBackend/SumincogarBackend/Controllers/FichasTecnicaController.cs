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

        [HttpGet("{categoriaId}")]
        public async Task<ActionResult<IEnumerable<BuscarFichaTecnica>>> GetFichatecnicaXCategoria(int categoriaId)
        {
            var fichasTecnicas = await _context.Fichatecnica.Include(x => x.Categoria)
                .Where(x => x.CategoriaId == categoriaId).ToListAsync();
            return _mapper.Map<List<BuscarFichaTecnica>>(fichasTecnicas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BuscarFichaTecnica>> GetFichatecnica(int id)
        {
            var fichatecnica = await _context.Fichatecnica.Include(x => x.Categoria)
                .FirstOrDefaultAsync(x => x.FichaTecnicaId == id);

            return _mapper.Map<BuscarFichaTecnica>(fichatecnica);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BuscarFichaTecnica>> PutFichatecnica(int id, CrearFichaTecnica crearFichaTecnica)
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

            fichaTecnica = await _context.Fichatecnica.Include(x => x.Categoria)
                .FirstOrDefaultAsync(x => x.FichaTecnicaId == id);

            return _mapper.Map<BuscarFichaTecnica>(fichaTecnica);
        }

        [HttpPost]
        public async Task<ActionResult<BuscarFichaTecnica>> PostFichatecnica(CrearFichaTecnica crearFichaTecnica)
        {
            var fichaTecnica = _mapper.Map<Fichatecnica>(crearFichaTecnica);

            if (crearFichaTecnica.DocumentoUrl != null)
            {
                fichaTecnica!.DocumentoUrl = await _cargarArchivos.CargarArchivo(TiposArchivo.FichaTecnica, crearFichaTecnica.DocumentoUrl);
            }

            _context.Fichatecnica.Add(fichaTecnica);
            await _context.SaveChangesAsync();

            fichaTecnica = await _context.Fichatecnica.Include(x => x.Categoria)
                .FirstOrDefaultAsync(x => x.FichaTecnicaId == fichaTecnica.FichaTecnicaId);

            return _mapper.Map<BuscarFichaTecnica>(fichaTecnica);
        }        
    }
}
