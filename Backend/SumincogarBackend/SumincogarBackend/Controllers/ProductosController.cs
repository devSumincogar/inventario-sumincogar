using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SumincogarBackend.Contexts;
using SumincogarBackend.DTO.CategoriaDTO;
using SumincogarBackend.DTO.ProductoDTO;
using SumincogarBackend.Models;
using SumincogarBackend.Services.AlmacenadorArchivos;
using SumincogarBackend.Services.CargarArchivos;

namespace SumincogarBackend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly db_a977c3_sumincogarContext _context;
        private readonly IMapper _mapper;
        private readonly ICargarArchivos _cargarArchivos;

        

        public ProductosController(db_a977c3_sumincogarContext context, IMapper mapper, ICargarArchivos cargarArchivos)
        {
            _context = context;
            _mapper = mapper;
            _cargarArchivos = cargarArchivos;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<BuscarProducto>>> GetProductos([FromQuery] int categoriaId)
        {
            var productos = await _context.Productos.Include(x => x.Categoria)
                .Where(x => x.Categoriaid == categoriaId).ToListAsync();

            return _mapper.Map<List<BuscarProducto>>(productos);
        }

        [HttpGet("{productoId}")]
        public async Task<ActionResult<BuscarProducto>> GetProducto(int productoId)
        {
            var producto = await _context.Productos.Include(x => x.Categoria)
                .FirstOrDefaultAsync(x => x.Productoid == productoId);

            if (producto == null) return NotFound();

            return _mapper.Map<BuscarProducto>(producto);
        }

        [HttpPut("{productoId}")]
        public async Task<ActionResult<BuscarProducto>> PutProducto(int productoId, [FromForm] CrearProducto crearProducto)
        {
            if (await ExistNameOrCode(crearProducto)) return BadRequest($"Ya existe el producto con nombre {crearProducto.Productonombre} o código {crearProducto.Codigo}");

            var producto = await _context.Productos.FindAsync(productoId);
            producto = _mapper.Map(crearProducto, producto);

            if (crearProducto.Imagenreferencial != null)
            {
                producto!.Imagenreferencial = await _cargarArchivos.ActualizarArchivo(TiposArchivo.ImagenProducto, crearProducto.Imagenreferencial, producto.Imagenreferencial!);                
            }

            if (crearProducto.Fichatenicapdf != null)
            {
                producto!.Fichatenicapdf = await _cargarArchivos.ActualizarArchivo(TiposArchivo.ImagenProducto, crearProducto.Fichatenicapdf, producto.Fichatenicapdf!);
            }

            try
            {
                _context.Entry(producto!).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            producto = await _context.Productos.Include(x => x.Categoria)
                .FirstOrDefaultAsync(x => x.Productoid == productoId);

            return _mapper.Map<BuscarProducto>(producto);
        }

        [HttpPost]
        public async Task<ActionResult<BuscarProducto>> PostProducto([FromForm] CrearProducto crearProducto)
        {
            if (await ExistNameOrCode(crearProducto)) return BadRequest($"Ya existe el producto con nombre {crearProducto.Productonombre} o código {crearProducto.Codigo}");

            var producto = _mapper.Map<Producto>(crearProducto);

            if (crearProducto.Imagenreferencial != null)
            {
                producto!.Imagenreferencial = await _cargarArchivos.CargarArchivo(TiposArchivo.ImagenProducto, crearProducto.Imagenreferencial);
            }

            if (crearProducto.Fichatenicapdf != null)
            {
                producto!.Fichatenicapdf = await _cargarArchivos.CargarArchivo(TiposArchivo.ImagenProducto, crearProducto.Fichatenicapdf);
            }

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            producto = await _context.Productos.Include(x => x.Categoria)
                .FirstOrDefaultAsync(x => x.Productoid == producto.Productoid);

            return _mapper.Map<BuscarProducto>(producto);
        }

        private async Task<bool> ExistNameOrCode(CrearProducto producto)
        {
            return await _context.Productos.AnyAsync(x => x.Productonombre.Equals(producto.Productonombre) || x.Codigo.Equals(producto.Codigo));
        }
    }
}
