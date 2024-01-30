using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SumincogarBackend.Contexts;
using SumincogarBackend.DTO.DetalleInventarioDTO;
using SumincogarBackend.DTO.FichaTecnicaDTO;
using SumincogarBackend.DTO.ImagenReferencialDTO;
using SumincogarBackend.DTO.ProductoDTO;
using SumincogarBackend.Models;
using SumincogarBackend.Services.CargarArchivos;
using System;
using System.Linq;

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
        public async Task<ActionResult<IEnumerable<BuscarProducto>>> GetProductos([FromQuery] int? subCategoriaId, [FromQuery] List<int>? gamasColor)
        {

            var productosGamaColor = new List<int>();

            if (gamasColor != null)
            {
                productosGamaColor = await _context.ProductoGamacolor
                    .Where(x => gamasColor.Contains(x.GamaColorId))
                    .Select(x => x.ProductoId).Distinct().ToListAsync();
            }

            var productos = await _context.Producto
                .Include(x => x.Subcategoria)
                .Include(x => x.ProductoGamacolor).ThenInclude(x => x.GamaColor)
                .Where(x => x.SubcategoriaId == subCategoriaId || subCategoriaId == null)
                .Where(x => productosGamaColor.Contains(x.ProductoId) || productosGamaColor == null)
                .ToListAsync();

            return _mapper.Map<List<BuscarProducto>>(productos);
        }

        [HttpGet("info/{codigo}")]
        public async Task<ActionResult<BuscarProducto>> GetProducto(string codigo)
        {
            var producto = await _context.Producto
                .Include(x => x.Subcategoria)
                .Include(x => x.ProductoGamacolor).ThenInclude(x => x.GamaColor)
                .FirstOrDefaultAsync(x => x.Codigo.Equals(codigo)
            );

            if (producto == null) return NotFound();

            return _mapper.Map<BuscarProducto>(producto);
        }

        [HttpGet("enStock")]
        public async Task<ActionResult<InfoGeneralProducto>> GetDetalleInventario([FromQuery] string codigo)
        {
            var producto = await _context.Producto
               .Include(x => x.Subcategoria)
               .Where(x => x.Codigo == codigo).FirstAsync();

            if (producto == null) return BadRequest("No existe el producto");

            var detalleInventario = await _context.Detalleinventario
                .Where(x => x.CodCliente == codigo).ToListAsync();

            if (detalleInventario == null) return NotFound("No Existe en el Inventario");

            var infoGeneralProducto = new InfoGeneralProducto
            {
                CodCliente = detalleInventario[0].CodCliente,
                Descontinuada = detalleInventario[0].Descontinuada ?? false,
                TelasSimilares = detalleInventario[0].TelasSimilares,
                CategoriaId = producto.Subcategoria!.CategoriaId,
                SubCategoriaId = producto.SubcategoriaId,
            };

            if (infoGeneralProducto.Descontinuada == true) return infoGeneralProducto;

            foreach (var detalle in detalleInventario)
            {
                var productoInventario = new ProductoInventario
                {
                    Codigo = producto.Codigo,
                    SubCategoriaId = producto.SubcategoriaId ?? 0,
                    SubCategoriaNombre = producto.Subcategoria!.SubcategoriaNombre ?? "",
                    Colores = detalle.Colores,
                    Impresion = detalle.Impresion,
                    Stock = detalle.Stock,
                    ProductoNombre = detalle.ProductoNombre ?? "",
                    Imagen = producto.ImagenUrl,
                    Orden = detalle.Stock!.Equals("ALTO") ? 1 : detalle.Stock!.Equals("MEDIO") ? 2 : detalle.Stock!.Equals("BAJO") ? 3 : 4
                };

                infoGeneralProducto.Productos.Add(productoInventario);
                var imagenes = await _context.Imagenreferencial.Where(x => x.CodCliente!.Equals(codigo))
                .Select(x => new Imagen
                {
                    ImagenReferenciaId = x.ImagenReferenciaId,
                    Url = x.Url
                })
                .ToListAsync();
                infoGeneralProducto.Imagenes.AddRange(imagenes);
            }

            infoGeneralProducto.Productos = infoGeneralProducto.Productos.OrderBy(x => x.Orden).ToList();


            var fichaTecnica = await _context.Fichatecnica.Where(x => x.CodCliente == codigo).FirstOrDefaultAsync();
            infoGeneralProducto.FichaTecnica = _mapper.Map<BuscarFichaTecnica>(fichaTecnica);

            return infoGeneralProducto;
        }


        [HttpGet("coloresSimilares")]
        public async Task<ActionResult<IEnumerable<BuscarProducto>>> GetColoresSimilares([FromQuery] string codigo)
        {
            var producto = await _context.Producto.Where(x => x.Codigo.Equals(codigo)).FirstAsync();

            if (producto == null) return NotFound("No Existe en el Inventario");


            var coloresId = await _context.ProductoGamacolor.Where(x => x.ProductoId == producto.ProductoId).Select(x => x.GamaColorId).ToListAsync();

            var productosSimilares = await _context.ProductoGamacolor.Include(x => x.Producto)
                .Where(x => coloresId.Contains(x.GamaColorId))
                .Select(x => x.Producto)
                .ToListAsync();

            if (productosSimilares.Count > 20)
            {
                Random random = new();
                // Obtener 20 elementos aleatorios de la lista
                var elementosAleatorios = productosSimilares.OrderBy(x => random.Next()).Take(20).ToList();

                return _mapper.Map<List<BuscarProducto>>(elementosAleatorios);
            }

            return _mapper.Map<List<BuscarProducto>>(productosSimilares);
        }

        [HttpGet("productosSimilares")]
        public async Task<ActionResult<IEnumerable<BuscarProducto>>> GetProductosSimilares([FromQuery] string codigo)
        {

            var producto = await _context.Producto.Where(x => x.Codigo.Equals(codigo)).FirstAsync();
            
            var productosSimilares = await _context.Producto.Where(x => x.SubcategoriaId == producto.SubcategoriaId)
                .Select(x => x.Codigo).ToListAsync();

            var productosEnStock = await _context.Detalleinventario
                .Where(x => productosSimilares.Contains(x.CodProducto!)).Select(x => x.CodProducto).ToListAsync();

            var productos = await _context.Producto.Where(x => productosEnStock.Contains(x.Codigo)).ToListAsync();

            return _mapper.Map<List<BuscarProducto>>(productos);
        }

        [HttpPut("infoProducto/{productoId}")]
        public async Task<IActionResult> PutProducto(int productoId, [FromBody] CrearProducto crearProducto)
        {            
            var producto = await _context.Producto.FindAsync(productoId);
            producto = _mapper.Map(crearProducto, producto);

            try
            {
                _context.Entry(producto!).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPut("agregarImagen")]
        public async Task<IActionResult> PutAgregarImagenProducto([FromForm] UpdateImgProducto updateImg)
        {
            var producto = await _context.Producto.Where(x => x.Codigo.Equals(updateImg.Codigo)).FirstOrDefaultAsync();

            if(producto == null) return  BadRequest("Crear Producto");

            producto!.ImagenUrl = await _cargarArchivos.ActualizarArchivo(TiposArchivo.ImagenProducto, updateImg.ImagenUrl!, producto.ImagenUrl!);
            
            try
            {
                _context.Entry(producto!).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> PostProducto([FromForm] CrearProducto crearProducto)
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

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Producto.FindAsync(id);
            if (producto == null) return BadRequest("No existe el producto");

            if(producto.ImagenUrl == null)
            {
                await _cargarArchivos.BorrarArchivo(TiposArchivo.ImagenProducto, producto.ImagenUrl!);
            }

            var productosGamaColor = await _context.ProductoGamacolor.Where(x => x.ProductoId == id).ToListAsync();

            if (productosGamaColor.Any())
            {
                _context.ProductoGamacolor.RemoveRange(productosGamaColor);
                await _context.SaveChangesAsync();
            }

            _context.Producto.Remove(producto);
            await _context.SaveChangesAsync();

            return Ok();
        }
        
        private async Task<bool> ExistNameOrCode(CrearProducto producto)
        {
            return await _context.Producto.AnyAsync(x => x.ProductoNombre.Equals(producto.ProductoNombre) || x.Codigo.Equals(producto.Codigo));
        }
    }
}
