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
using SumincogarBackend.Models;

namespace SumincogarBackend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly db_a977c3_sumincogarContext _context;
        private readonly IMapper _mapper;

        public CategoriasController(db_a977c3_sumincogarContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BuscarCategoria>>> GetCategoria()
        {
            var categorias = await _context.Categoria.ToListAsync();
            return _mapper.Map<List<BuscarCategoria>>(categorias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BuscarCategoria>> GetCategorium(int id)
        {
            var categoria = await _context.Categoria.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return _mapper.Map<BuscarCategoria>(categoria);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BuscarCategoria>> PutCategorium(int id, CrearCategoria crearCategoria)
        {
            if (await ExistName(crearCategoria.Categorianombre)) return BadRequest($"Ya existe la categoría {crearCategoria.Categorianombre}");

            var categoria = await _context.Categoria.FindAsync(id);
            categoria = _mapper.Map(crearCategoria, categoria);

            try
            {
                _context.Entry(categoria!).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return _mapper.Map<BuscarCategoria>(categoria);
        }

        [HttpPost]
        public async Task<ActionResult<BuscarCategoria>> PostCategorium(CrearCategoria crearCategoria)
        {
            if (await ExistName(crearCategoria.Categorianombre)) return BadRequest($"Ya existe la categoría {crearCategoria.Categorianombre}");

            var categoria = _mapper.Map<Categoria>(crearCategoria);

            _context.Categoria.Add(categoria);
            await _context.SaveChangesAsync();

            return _mapper.Map<BuscarCategoria>(categoria);
        }

        private async Task<bool> ExistName(string name)
        {
            return await _context.Categoria.AnyAsync(x => x.CategoriaNombre.Equals(name));
        }
    }
}
