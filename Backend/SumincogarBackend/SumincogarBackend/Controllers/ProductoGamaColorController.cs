using System;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SumincogarBackend.Contexts;
using SumincogarBackend.DTO.GamaColorDTO;

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

        //[HttpGet("producto")]
        //public async Task<IEnumerable<BuscarGamaColor>> GetGamasColorProducto([FromQuery] int productoId, [FromQuery] bool gamaAsignadas)
        //{
        //    if (gamaAsignadas)
        //    {
        //        var gamasColor = await _context.ProductoGamacolor.Include(x => x.)

        //    }
        //}
    }
}

