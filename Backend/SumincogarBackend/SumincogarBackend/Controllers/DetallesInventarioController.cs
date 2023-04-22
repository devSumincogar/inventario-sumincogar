using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SumincogarBackend.Contexts;
using SumincogarBackend.DTO.DetalleInventarioDTO;
using SumincogarBackend.Models;
using SumincogarBackend.Services.CargarArchivos;
using System.Text;

namespace SumincogarBackend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class DetallesInventarioController : ControllerBase
    {
        private readonly db_a977c3_sumincogarContext _context;
        private readonly IMapper _mapper;

        public DetallesInventarioController(db_a977c3_sumincogarContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //[HttpGet]
        //public async Task<ActionResult<BuscarDetalleInventario>> GetDetalleInventario([FromQuery] string Codigo)
        //{

        //}

        [HttpPost]
        public async Task<IActionResult> CargarDetallesInventario(IFormFile files)
        {
            if (files.Length > 0)
            {
                var detalles = new List<Detalleinventario>();

                var st = new MemoryStream();
                await files.CopyToAsync(st);

                var content = Encoding.UTF8.GetString(st.ToArray());
                string[] datos = content.Split("\n");
               
                foreach (var item in datos)
                {                    
                    if (!string.IsNullOrEmpty(item))
                    {
                        var cells = item.Split(",");
                        
                        var detalle = new Detalleinventario
                        {
                            CodCliente = cells[0],
                            CodProducto = cells[1],
                            Stock = cells[2],
                            Impresion = cells[3],
                            Colores = cells[4],                            
                        };

                        detalles.Add(detalle);
                    }
                }

                if(detalles.Count > 0)
                {
                    var detallesBorrar = await _context.Detalleinventario.ToListAsync();
                    if (detallesBorrar.Count > 0)
                    {
                        _context.Detalleinventario.RemoveRange(detallesBorrar);
                        await _context.SaveChangesAsync();
                    }
                }

                _context.Detalleinventario.AddRange(detalles);
                await _context.SaveChangesAsync();

                return Ok();
            }
            else
            {
                return BadRequest("ERROR");
            }
        }
    }
}
