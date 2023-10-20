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
        public async Task<IEnumerable<BuscarUsuarioDTO>> GetUsuarios()
        {
            var usuarios = await _context.Usuario
                .OrderBy(x => x.Apellido).ThenBy(x => x.Nombre)
                .ToListAsync();

            return _mapper.Map<List<BuscarUsuarioDTO>>(usuarios);
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar(CrearUsuarioDTO credencialesUsuario)
        {
            var user = await _userManager.FindByEmailAsync(credencialesUsuario.Email);

            if(user != null) return BadRequest("El usuario ya se encuentra registrado");

            var identityUser = new IdentityUser { 
                UserName = credencialesUsuario.Email, 
                Email = credencialesUsuario.Email,
            };

            var newPassword = _generadorStrings.GenerateRandomString(6);

            var result = await _userManager.CreateAsync(identityUser, newPassword);            

            if(result.Succeeded)
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
