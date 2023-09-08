using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SumincogarBackend.Contexts;
using SumincogarBackend.DTO.GamaColorDTO;
using SumincogarBackend.DTO.SubCategoriaDTO;
using SumincogarBackend.Models;

namespace SumincogarBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GamasColorController : ControllerBase
    {
        private readonly db_a977c3_sumincogarContext _context;
        private readonly IMapper _mapper;

        public GamasColorController(db_a977c3_sumincogarContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BuscarGamaColor>>> GetGamaColor()
        {
            var GamaColors = await _context.GamaColor.OrderBy(x => x.GamaColorNombre).ToListAsync();
            return _mapper.Map<List<BuscarGamaColor>>(GamaColors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BuscarGamaColor>> GetCategorium(int id)
        {
            var GamaColor = await _context.GamaColor.FindAsync(id);

            if (GamaColor == null)
            {
                return NotFound();
            }

            return _mapper.Map<BuscarGamaColor>(GamaColor);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BuscarGamaColor>> PutCategorium(int id, CrearGamaColor crearGamaColor)
        {
            if (await ExistName(crearGamaColor.GamaColorNombre!)) return BadRequest($"Ya existe la categoría {crearGamaColor.GamaColorNombre}");

            var GamaColor = await _context.GamaColor.FindAsync(id);
            GamaColor = _mapper.Map(crearGamaColor, GamaColor);

            try
            {
                _context.Entry(GamaColor!).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return _mapper.Map<BuscarGamaColor>(GamaColor);
        }

        [HttpPost]
        public async Task<ActionResult<BuscarGamaColor>> PostCategorium(CrearGamaColor crearGamaColor)
        {
            if (await ExistName(crearGamaColor.GamaColorNombre!)) return BadRequest($"Ya existe la categoría {crearGamaColor.GamaColorNombre}");

            var GamaColor = _mapper.Map<GamaColor>(crearGamaColor);

            _context.GamaColor.Add(GamaColor);
            await _context.SaveChangesAsync();

            return _mapper.Map<BuscarGamaColor>(GamaColor);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BuscarSubCategoria>> DeleteGamaColor(int id)
        {
            var productosGamaColor = await _context.ProductoGamacolor
                .Where(x => x.GamaColorId! == id).ToListAsync();

            if (productosGamaColor.Any())
            {
                _context.ProductoGamacolor.RemoveRange(productosGamaColor);
                await _context.SaveChangesAsync();
            }

            var gamaColor = await _context.GamaColor.FindAsync(id);

            _context.GamaColor.Remove(gamaColor!);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private async Task<bool> ExistName(string name)
        {
            return await _context.GamaColor.AnyAsync(x => x.GamaColorNombre!.Equals(name));
        }
    }
}
