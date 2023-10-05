using System;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SumincogarBackend.Contexts;
using SumincogarBackend.DTO.GamaColorDTO;
using SumincogarBackend.DTO.ProductoGamaColorDTO;
using SumincogarBackend.Models;

namespace SumincogarBackend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ProductoGamaColorController : ControllerBase
    {
        private readonly db_a977c3_sumincogarContext _context;
        private readonly IMapper _mapper;

        public ProductoGamaColorController(db_a977c3_sumincogarContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("producto")]
        public async Task<IEnumerable<BuscarGamaColor>> GetGamasColorProducto([FromQuery] int productoId, [FromQuery] bool gamaAsignadas)
        {
            if (gamaAsignadas)
            {
                var gamasColor = await _context.ProductoGamacolor.Include(x => x.GamaColor)
                    .Where(x => x.ProductoId == productoId)
                    .Select(x => x.GamaColor)
                    .OrderBy(x => x.GamaColorNombre)
                    .ToListAsync();

                return _mapper.Map<List<BuscarGamaColor>>(gamasColor);
            }
            else
            {
                var gamasColorId = await _context.ProductoGamacolor
                    .Where(x => x.ProductoId == productoId)
                    .Select(x => x.GamaColorId).ToListAsync();

                var gamasColor = await _context.GamaColor
                    .Where(x => !gamasColorId.Contains(x.GamaColorId))
                    .OrderBy(x => x.GamaColorNombre)
                    .ToListAsync();

                return _mapper.Map<List<BuscarGamaColor>>(gamasColor);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> PostGamasColorProducto([FromBody] CrearProductoGamaColor crearProductoGamaColor)
        {
            var productoGamaColor = new ProductoGamacolor { GamaColorId = crearProductoGamaColor.GamaColorId, ProductoId = crearProductoGamaColor.ProductoId };

            _context.ProductoGamacolor.Add(productoGamaColor);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpDelete("{productoId}/{gamaColorId}")]
        public async Task<IActionResult> DeleteGamasColorProducto(int productoId, int gamaColorId)
        {
            var productoGamaColor = await _context.ProductoGamacolor.Where(x => x.ProductoId == productoId).Where(x => x.GamaColorId == gamaColorId).FirstOrDefaultAsync();

            if (productoGamaColor == null) return BadRequest();

            _context.ProductoGamacolor.Remove(productoGamaColor);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

