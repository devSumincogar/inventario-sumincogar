using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using OfficeOpenXml;
using SumincogarBackend.Contexts;
using SumincogarBackend.DTO.ProductoDTO;
using SumincogarBackend.DTO.UsuariosDTO;
using SumincogarBackend.Models;
using SumincogarBackend.Services.CargarArchivos;
using SumincogarBackend.Services.EnviarEmails;
using SumincogarBackend.Services.GeneradorStrings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace SumincogarBackend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly db_a977c3_sumincogarContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IGeneradorStrings _generadorStrings;
        private readonly IEnviarEmail _enviarEmail;

        public UsuariosController(db_a977c3_sumincogarContext context, IMapper mapper, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration, IGeneradorStrings generadorStrings, IEnviarEmail enviarEmail)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _generadorStrings = generadorStrings;
            _enviarEmail = enviarEmail;
        }

        [HttpGet()]
        public async Task<IEnumerable<BuscarUsuarioDTO>> GetUsuarios([FromQuery] string email)
        {
            var usuarios = await _context.Usuario
                .OrderBy(x => x.Nombre).ThenBy(x => x.Apellido)
                .Where(x => x.Email!.Equals(email))
                .ToListAsync();

            return _mapper.Map<List<BuscarUsuarioDTO>>(usuarios);
        }


        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar(CrearUsuarioDTO credencialesUsuario)
        {
            var user = await _userManager.FindByEmailAsync(credencialesUsuario.Email);

            if (user != null) return BadRequest("El usuario ya se encuentra registrado");

            var identityUser = new IdentityUser {
                UserName = credencialesUsuario.Email,
                Email = credencialesUsuario.Email,
            };

            var newPassword = _generadorStrings.GenerateRandomString(6);

            var result = await _userManager.CreateAsync(identityUser, newPassword);

            if (result.Succeeded)
            {
                user = await _userManager.FindByEmailAsync(credencialesUsuario.Email);

                _context.Usuario.Add(new Usuario
                {
                    UsuarioId = user.Id,
                    Apellido = credencialesUsuario.Apellido,
                    Nombre = credencialesUsuario.Nombre,
                    Tutorial = false,
                    ResetPassword = true,
                });

                await _context.SaveChangesAsync();

                string body = $"Bienvenido a SumincogarApp, se ha registrado su usuario con el email {credencialesUsuario.Email} y la contraseña para ingresar es: {newPassword}. Al ingresar al app se le solicitará cambiar por una nueva contraseña";

                var isEmailConfirmed = _enviarEmail.SendEmail(credencialesUsuario.Email, "Usuario Registrado Sumincogar App", body);
                return isEmailConfirmed ? Ok() : BadRequest();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("registrarConPassword")]
        public async Task<IActionResult> RegistrarConPassword(CrearUsuarioDTO credencialesUsuario)
        {
            var user = await _userManager.FindByEmailAsync(credencialesUsuario.Email);

            if (user != null) return BadRequest("El usuario ya se encuentra registrado");

            var identityUser = new IdentityUser
            {
                UserName = credencialesUsuario.Email,
                Email = credencialesUsuario.Email,
            };

            var result = await _userManager.CreateAsync(identityUser, credencialesUsuario.Password);

            if (result.Succeeded)
            {
                user = await _userManager.FindByEmailAsync(credencialesUsuario.Email);

                _context.Usuario.Add(new Usuario
                {
                    UsuarioId = user.Id,
                    Apellido = credencialesUsuario.Apellido,
                    Nombre = credencialesUsuario.Nombre,
                    Tutorial = false,
                    ResetPassword = true,
                    Email = credencialesUsuario.Email,
                });

                await _context.SaveChangesAsync();

                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("excel")]
        [RequestFormLimits(ValueCountLimit = int.MaxValue, MultipartBodyLengthLimit = long.MaxValue)]
        [DisableRequestSizeLimit]
        public async Task<ActionResult<dynamic>> CargarUsuariosEXCEL(IFormFile file)
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
                        if ((worksheet.Cells[row, 5].Value.ToString() ?? "").Equals("")) continue;

                        var user = await _userManager.FindByEmailAsync(worksheet.Cells[row, 5].Value.ToString());

                        if (user != null) continue;

                        var identityUser = new IdentityUser
                        {
                            UserName = worksheet.Cells[row, 5].Value.ToString(),
                            Email = worksheet.Cells[row, 5].Value.ToString(),
                        };

                        var result = await _userManager.CreateAsync(identityUser, worksheet.Cells[row,4].Value.ToString());

                        if (result.Succeeded)
                        {
                            user = await _userManager.FindByEmailAsync(worksheet.Cells[row, 5].Value.ToString());

                            _context.Usuario.Add(new Usuario
                            {
                                UsuarioId = user.Id,
                                Apellido = worksheet.Cells[row, 2].Value.ToString() ?? "",
                                Nombre = worksheet.Cells[row, 3].Value.ToString() ?? "",
                                Tutorial = false,
                                ResetPassword = true,
                                Email = worksheet.Cells[row, 5].Value.ToString(),
                            });

                            await _context.SaveChangesAsync();
                        }
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

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<RespuestaLogin>> Login(CredencialesUsuario credencialesUsuario)
        {
            var resultado = await _signInManager.PasswordSignInAsync(credencialesUsuario.Email, 
                credencialesUsuario.Password, isPersistent: false, lockoutOnFailure: false);

            if(resultado.Succeeded ) {
                return await ConstruirToken(credencialesUsuario);
            }
            else
            {
                return BadRequest("Login Incorrecto");
            }
        }

        [HttpPut()]
        public async Task<IActionResult> PutUsuario([FromBody] UpdateUsuario updateUsuario)
        {
            var identityUser = await _userManager.FindByIdAsync(updateUsuario.UsuarioId);

            if (identityUser == null) return BadRequest();

            if (!updateUsuario.Email!.Equals(identityUser.Email))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(identityUser);
                var resultado = await _userManager.ChangeEmailAsync(identityUser, updateUsuario.Email, token);
            }

            var usuario = await _context.Usuario.Where(x => x.UsuarioId.Equals(updateUsuario.UsuarioId)).FirstAsync();

            usuario = _mapper.Map(updateUsuario, usuario);

            try
            {
                _context.Entry(usuario!).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
        }

        [HttpPut("tutorial")]
        public async Task<ActionResult<RespuestaLogin>> PutTutorialUsuario(EmailUsuario updateUsuario)
        {
            var user = await _userManager.FindByEmailAsync(updateUsuario.Email);

            var usuario = await _context.Usuario.Where(x => x.UsuarioId.Equals(user.Id)).FirstOrDefaultAsync();

            if (usuario == null) return BadRequest();

            usuario.Tutorial = true;

            try
            {
                _context.Entry(usuario!).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("recuperarContrasenia")]
        public async Task<IActionResult> PutRecuperarContrasenia([FromBody] EmailUsuario cambiarContraseniaUsuario)
        {
            var identityUser = await _userManager.FindByEmailAsync(cambiarContraseniaUsuario.Email);

            if (identityUser == null) return BadRequest();

            var token = await _userManager.GeneratePasswordResetTokenAsync(identityUser);

            var newPassword = _generadorStrings.GenerateRandomString(6);
            var resultado = await _userManager.ResetPasswordAsync(identityUser, token, newPassword);

            if (resultado.Succeeded)
            {
                var usuario = await _context.Usuario.Where(x => x.UsuarioId.Equals(identityUser.Id)).FirstOrDefaultAsync();
                usuario!.ResetPassword = true;

                try
                {
                    _context.Entry(usuario!).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    string body = $"La nueva contraseña para ingresar es: {newPassword}. Al ingresar al app se le solicitará cambiar por una nueva contraseña";

                    var isEmailConfirmed = _enviarEmail.SendEmail(cambiarContraseniaUsuario.Email, "Nueva Contraseña App Sumincogar", body);
                    return isEmailConfirmed ? Ok() : BadRequest();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest();
                }                
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }

        [HttpPut("cambiarContrasenia")]
        public async Task<IActionResult> PutCambiarContrasenia([FromBody] CambiarContraseniaDTO cambiarContraseniaUsuario)
        {
            var identityUser = await _userManager.FindByEmailAsync(cambiarContraseniaUsuario.Email);
            var token = await _userManager.GeneratePasswordResetTokenAsync(identityUser);

            var resultado = await _userManager.ResetPasswordAsync(identityUser, token, cambiarContraseniaUsuario.NewPassword);

            if (resultado.Succeeded)
            {
                var usuario = await _context.Usuario.Where(x => x.UsuarioId.Equals(identityUser.Id)).FirstOrDefaultAsync();
                usuario!.ResetPassword = false;

                try
                {
                    _context.Entry(usuario!).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return Ok();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }

        private async Task<RespuestaLogin> ConstruirToken(CredencialesUsuario credencialesUsuario)
        {
            var user = await _userManager.FindByEmailAsync(credencialesUsuario.Email);
            var usuario = await _context.Usuario.Where(x => x.UsuarioId.Equals(user.Id)).FirstAsync();

            var claims = new List<Claim>()
            {
                new Claim("email", credencialesUsuario.Email),
                new Claim("nombre", usuario.Nombre),
                new Claim("tutorial", usuario.Tutorial.ToString()),
            };

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddHours(24);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: creds);

            return new RespuestaLogin
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiracion,
                ResetPassword = usuario.ResetPassword
            };
        }
        
    }
}
