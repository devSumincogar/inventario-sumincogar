using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SumincogarBackend.Contexts;
using SumincogarBackend.Models;
using System.Text;

namespace SumincogarBackend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class DetallesInventarioController : ControllerBase
    {
        private readonly db_a977c3_sumincogarContext _context;

        public DetallesInventarioController(db_a977c3_sumincogarContext context)
        {
            _context = context;
        }

        
        [HttpPost("csv")]
        public async Task<IActionResult> CargarDetallesInventarioCSV(IFormFile files)
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

                        if (cells[0].TrimStart('\uFEFF').Equals("CODIGO")) continue;


                        var detalle = new Detalleinventario
                        {
                            CodProducto = cells[0].TrimStart('\uFEFF').Trim(),
                            CodCliente = cells[2].TrimStart('\uFEFF').Trim(),
                            Stock = cells[3].Trim(),
                            Impresion = cells[4].Trim(),
                            Descontinuada = cells[5].Trim().Equals("SI"),
                            TelasSimilares = cells[6].Replace("\r", "")
                        };

                        detalles.Add(detalle);
                    }
                }

                if (detalles.Count > 0)
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

        [HttpPost("excel")]
        public async Task<ActionResult<dynamic>> CargarDetallesInventarioEXCEL(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var detalles = new List<Detalleinventario>();

                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Selecciona la primera hoja del archivo Excel (índice 0).

                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var detalle = new Detalleinventario
                        {
                            CodProducto = worksheet.Cells[row, 1].Value == null ? "" : worksheet.Cells[row, 1].Value.ToString() ?? "",
                            ProductoNombre = worksheet.Cells[row, 2].Value == null ? "" : worksheet.Cells[row, 2].Value.ToString() ?? "",
                            CodCliente = worksheet.Cells[row, 3].Value == null ? "" : worksheet.Cells[row, 3].Value.ToString() ?? "",
                            Stock = worksheet.Cells[row, 4].Value == null ? "" : worksheet.Cells[row, 4].Value.ToString() ?? "",
                            Impresion = worksheet.Cells[row, 5].Value == null ? "" : worksheet.Cells[row, 5].Value.ToString() ?? "",
                            Descontinuada = worksheet.Cells[row,6].Value != null && worksheet.Cells[row, 6].Value.ToString()!.Equals("1"),
                            TelasSimilares = worksheet.Cells[row, 7].Value == null ? "" : worksheet.Cells[row, 7].Value.ToString() ?? "",
                        };
                        detalles.Add(detalle);
                    }
                }

                if (detalles.Count > 0)
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

            return BadRequest("No se proporcionó un archivo Excel válido.");
        }
    }
}
