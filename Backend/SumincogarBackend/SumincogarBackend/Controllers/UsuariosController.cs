using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
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

namespace SumincogarBackend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly db_a977c3_sumincogarContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IGeneradorStrings _generadorStrings;
        private readonly IEnviarEmail _enviarEmail;

        public UsuariosController(db_a977c3_sumincogarContext context, UserManager<IdentityUser> userManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager, IGeneradorStrings generadorStrings, IEnviarEmail enviarEmail)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _generadorStrings = generadorStrings;
            _enviarEmail = enviarEmail;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<RespuestaLogin>> Registrar(CrearUsuarioDTO credencialesUsuario)
        {
            var user = await _userManager.FindByEmailAsync(credencialesUsuario.Email);

            if(user != null) return BadRequest("El usuario ya se encuentra registrado");

            var identityUser = new IdentityUser { 
                UserName = credencialesUsuario.Email, 
                Email = credencialesUsuario.Email,
            };
            var result = await _userManager.CreateAsync(identityUser, credencialesUsuario.Password);

            if(result.Succeeded)
            {
                user = await _userManager.FindByEmailAsync(credencialesUsuario.Email);

                _context.Usuario.Add(new Usuario
                {
                    UsuarioId = user.Id,
                    Apellido = credencialesUsuario.Apellido,
                    Nombre = credencialesUsuario.Nombre,
                    Tutorial = false
                });

                await _context.SaveChangesAsync();

                return await ConstruirToken(new CredencialesUsuario { Email = user.Email, Password = credencialesUsuario.Password});
            }
            else
            {
                return BadRequest(result.Errors);
            }
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

        [HttpPut("tutorial")]
        public async Task<ActionResult<RespuestaLogin>> PutUsuario(UpdateUsuario updateUsuario)
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
        public async Task<IActionResult> PutRecuperarContrasenia([FromBody] UpdateUsuario cambiarContraseniaUsuario)
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

            var expiracion = DateTime.UtcNow.AddHours(12);

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
