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
                .Include(x => x.Imagenreferencial)
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
                .Include(x => x.Imagenreferencial)
                .Include(x => x.Subcategoria)
                .Include(x => x.ProductoGamacolor).ThenInclude(x => x.GamaColor)
                .FirstOrDefaultAsync(x => x.Codigo.Equals(codigo)
            );

            if (producto == null) return NotFound();

            return _mapper.Map<BuscarProducto>(producto);
        }

        [HttpGet("codigo")]
        public async Task<ActionResult<InfoGeneralProducto>> GetProductoPorCodigo([FromQuery] string codigo)
        {
            var infoGeneralProducto = new InfoGeneralProducto();

            var producto = await _context.Producto
                .Include(x => x.Imagenreferencial)
                .Include(x => x.Subcategoria)
                .Where(x => x.Codigo == codigo).FirstAsync();

            if (producto == null) return BadRequest();

            var detalle = await _context.Detalleinventario.Where(x => x.CodProducto == codigo).FirstOrDefaultAsync();

            var productoInventario = new ProductoInventario
            {
                Codigo = producto.Codigo,
                SubCategoriaId = producto.SubcategoriaId ?? 0,
                SubCategoriaNombre = producto.Subcategoria!.SubcategoriaNombre ?? "",
                Impresion = detalle == null ? "" : detalle.Impresion,
                Stock = detalle == null ? "" : detalle.Stock,
                ProductoNombre = producto.ProductoNombre,
                ProductoId = producto.ProductoId,
                Imagen = producto.ImagenUrl,
                Orden = detalle == null ? 4 : detalle.Stock!.Equals("ALTO") ? 1 : detalle.Stock!.Equals("MEDIO") ? 2 : detalle.Stock!.Equals("BAJO") ? 3 : 4
            };

            infoGeneralProducto.Productos.Add(productoInventario);
            infoGeneralProducto.Imagenes.AddRange(_mapper.Map<List<BuscarImagenRefencial>>(producto.Imagenreferencial));

            infoGeneralProducto.Productos = infoGeneralProducto.Productos.OrderBy(x => x.Orden).ToList();

            var fichaTecnica = await _context.Fichatecnica.Where(x => x.SubcategoriaId == producto.SubcategoriaId).FirstOrDefaultAsync();
            infoGeneralProducto.FichaTecnica = _mapper.Map<BuscarFichaTecnica>(fichaTecnica);

            return infoGeneralProducto;
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
                    //.Include(x => x.Imagenreferencial)
                    .Include(x => x.Subcategoria)
                    .Where(x => x.Codigo == detalle.CodProducto).FirstAsync();

                if (producto == null) continue;

                var productoInventario = new ProductoInventario
                {
                    Codigo = producto.Codigo,
                    SubCategoriaId = producto.SubcategoriaId ?? 0,
                    SubCategoriaNombre = producto.Subcategoria!.SubcategoriaNombre ?? "",
                    Colores = detalle.Colores,
                    Impresion = detalle.Impresion,
                    Stock = detalle.Stock,
                    ProductoNombre = producto.ProductoNombre,
                    ProductoId = producto.ProductoId,
                    Imagen = producto.ImagenUrl,
                    Orden = detalle.Stock!.Equals("ALTO") ? 1 : detalle.Stock!.Equals("MEDIO") ? 2 : detalle.Stock!.Equals("BAJO") ? 3 : 4
                };

                infoGeneralProducto.Productos.Add(productoInventario);
                //infoGeneralProducto.Imagenes.AddRange(_mapper.Map<List<BuscarImagenRefencial>>(producto.Imagenreferencial));
            }

            infoGeneralProducto.Productos = infoGeneralProducto.Productos.OrderBy(x => x.Orden).ToList();


            var fichaTecnica = await _context.Fichatecnica.Where(x => x.SubcategoriaId == infoGeneralProducto.Productos[0].SubCategoriaId).FirstOrDefaultAsync();
            infoGeneralProducto.FichaTecnica = _mapper.Map<BuscarFichaTecnica>(fichaTecnica);

            return infoGeneralProducto;
        }

        [HttpPut("{productoId}")]
        public async Task<IActionResult> PutProducto(int productoId, [FromForm] CrearProducto crearProducto)
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
