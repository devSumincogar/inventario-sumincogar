﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SumincogarBackend.Contexts;
using SumincogarBackend.DTO.CategoriaDTO;
using SumincogarBackend.DTO.SubCategoriaDTO;
using SumincogarBackend.Models;

namespace SumincogarBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SubCategoriaController : ControllerBase
    {
        private readonly db_a977c3_sumincogarContext _context;
        private readonly IMapper _mapper;

        public SubCategoriaController(db_a977c3_sumincogarContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{categoriaId}")]
        public async Task<ActionResult<IEnumerable<BuscarSubCategoria>>> GetSubCategoria(int categoriaId)
        {
            var subCategorias = await _context.SubCategoria.Where(x => x.CategoriaId == categoriaId).ToListAsync();
            return _mapper.Map<List<BuscarSubCategoria>>(subCategorias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BuscarSubCategoria>> GetCategorium(int id)
        {
            var subCategoria = await _context.SubCategoria.FindAsync(id);

            if (subCategoria == null)
            {
                return NotFound();
            }

            return _mapper.Map<BuscarSubCategoria>(subCategoria);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BuscarSubCategoria>> PutCategorium(int id, CrearSubCategoria crearSubCategoria)
        {
            if (await ExistName(crearSubCategoria.SubcategoriaNombre)) return BadRequest($"Ya existe la categoría {crearSubCategoria.SubcategoriaNombre}");

            var subCategoria = await _context.SubCategoria.FindAsync(id);
            subCategoria = _mapper.Map(crearSubCategoria, subCategoria);

            try
            {
                _context.Entry(subCategoria!).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return _mapper.Map<BuscarSubCategoria>(subCategoria);
        }

        [HttpPost]
        public async Task<ActionResult<BuscarSubCategoria>> PostCategorium(CrearSubCategoria crearSubCategoria)
        {
            if (await ExistName(crearSubCategoria.SubcategoriaNombre)) return BadRequest($"Ya existe la categoría {crearSubCategoria.SubcategoriaNombre}");

            var subCategoria = _mapper.Map<SubCategoria>(crearSubCategoria);

            _context.SubCategoria.Add(subCategoria);
            await _context.SaveChangesAsync();

            return _mapper.Map<BuscarSubCategoria>(subCategoria);
        }

        private async Task<bool> ExistName(string name)
        {
            return await _context.Categoria.AnyAsync(x => x.CategoriaNombre.Equals(name));
        }
    }
}