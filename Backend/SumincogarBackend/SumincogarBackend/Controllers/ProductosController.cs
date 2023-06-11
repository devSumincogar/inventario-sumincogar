using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SumincogarBackend.Contexts;
using SumincogarBackend.DTO.DetalleInventarioDTO;
using SumincogarBackend.DTO.FichaTecnicaDTO;
using SumincogarBackend.DTO.ProductoDTO;
using SumincogarBackend.Models;
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
        public async Task<ActionResult<IEnumerable<BuscarProducto>>> GetProductos([FromQuery] int? fichaTecnicaId)
        {
            var productos = await _context.Producto
                .Include(x => x.Imagenreferencial)
                .Where(x => x.FichaTecnicaId == fichaTecnicaId || fichaTecnicaId == null)
                .ToListAsync();

            return _mapper.Map<List<BuscarProducto>>(productos);
        }

        [HttpGet("{productoId}")]
        public async Task<ActionResult<BuscarProducto>> GetProducto(int productoId)
        {
            var producto = await _context.Producto.Include(x => x.Imagenreferencial).FirstOrDefaultAsync(x => x.ProductoId == productoId);

            if (producto == null) return NotFound();

            return _mapper.Map<BuscarProducto>(producto);
        }

        [HttpGet("enStock")]
        public async Task<ActionResult<InfoGeneralProducto>> GetDetalleInventario([FromQuery] string codigo)
        {
            var detalleInventario = await _context.Detalleinventario
                .Where(x => x.CodCliente == codigo).ToListAsync();

            if (detalleInventario == null) return NotFound("No Existe en el Inventario");

            var infoGeneralProducto = new InfoGeneralProducto();
            infoGeneralProducto.CodCliente = detalleInventario[0].CodCliente;
            
            foreach(var detalle in detalleInventario)
            {
                var producto = await _context.Producto
                    .Include(x => x.Imagenreferencial)
                    .Include(x => x.FichaTecnica).ThenInclude(x => x!.Parametrotecnico)
                    .Where(x => x.Codigo == detalle.CodProducto).FirstAsync();

                if (producto == null) continue;

                if (infoGeneralProducto.FichaTecnica.NombreFichaTecnica == "")
                {
                    infoGeneralProducto.FichaTecnica = _mapper.Map<BuscarFichaTecnica>(producto.FichaTecnica);
                }

                var productoInventario = new ProductoInventario
                {
                    Codigo = producto.Codigo,
                    Colores = detalle.Colores,
                    Impresion = detalle.Impresion,
                    Stock = detalle.Stock,
                    ProductoNombre = producto.ProductoNombre,
                    ProductoId = producto.ProductoId,
                    Imagen = producto.ImagenUrl,
                };

                infoGeneralProducto.Productos.Add(productoInventario);
                infoGeneralProducto.Imagenes.AddRange(_mapper.Map<List<BuscarImagenRefencial>>(producto.Imagenreferencial));
            }
        
            return infoGeneralProducto;
        }

        [HttpPut("{productoId}")]
        public async Task<ActionResult<BuscarProducto>> PutProducto(int productoId, [FromForm] CrearProducto crearProducto)
        {            
            var producto = await _context.Producto.FindAsync(productoId);
            producto = _mapper.Map(crearProducto, producto);

            if (crearProducto.ImagenUrl != null)
            {
                producto!.ImagenUrl = await _cargarArchivos.ActualizarArchivo(TiposArchivo.ImagenProducto, crearProducto.ImagenUrl!, producto.ImagenUrl!);
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

            producto = await _context.Producto.FirstOrDefaultAsync(x => x.ProductoId == productoId);

            return _mapper.Map<BuscarProducto>(producto);
        }

        [HttpPost]
        public async Task<ActionResult<BuscarProducto>> PostProducto([FromForm] CrearProducto crearProducto)
        {
            if (await ExistNameOrCode(crearProducto)) return BadRequest($"Ya existe el producto con nombre {crearProducto.ProductoNombre} o código {crearProducto.Codigo}");

            var producto = _mapper.Map<Producto>(crearProducto);

            if (crearProducto.ImagenUrl != null)
            {
                producto!.ImagenUrl = await _cargarArchivos.CargarArchivo(TiposArchivo.ImagenProducto, crearProducto.ImagenUrl);
            }

            _context.Producto.Add(producto);
            await _context.SaveChangesAsync();

            producto = await _context.Producto
                .FirstOrDefaultAsync(x => x.ProductoId == producto.ProductoId);

            return _mapper.Map<BuscarProducto>(producto);
        }

        [HttpPost("imagenReferencial")]
        public async Task<IActionResult> PostImagenReferencialProducto([FromForm] CrearImagenReferencial crearImagenReferencial)
        {
            var imagenReferencial = _mapper.Map<Imagenreferencial>(crearImagenReferencial);

            if(crearImagenReferencial.Url != null)
            {
                imagenReferencial!.Url = await _cargarArchivos.CargarArchivo(TiposArchivo.ImagenProducto, crearImagenReferencial!.Url!);
            }

            _context.Imagenreferencial.Add(imagenReferencial);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("imagenReferencial/{id}")]
        public async Task<IActionResult> DeleteImagenReferencialProducto(int id)
        {
            if (_context.Imagenreferencial == null)
            {
                return NotFound();
            }
            var imagenReferencial = await _context.Imagenreferencial.FindAsync(id);
            if (imagenReferencial == null)
            {
                return NotFound();
            }

            if (imagenReferencial.Url != null)
            {
                await _cargarArchivos.BorrarArchivo(TiposArchivo.Catalogo, imagenReferencial.Url);
            }

            _context.Imagenreferencial.Remove(imagenReferencial);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private async Task<bool> ExistNameOrCode(CrearProducto producto)
        {
            return await _context.Producto.AnyAsync(x => x.ProductoNombre.Equals(producto.ProductoNombre) || x.Codigo.Equals(producto.Codigo));
        }
    }
}
