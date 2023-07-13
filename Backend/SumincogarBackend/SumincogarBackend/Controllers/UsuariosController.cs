using AutoMapper;
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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SumincogarBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly db_a977c3_sumincogarContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMapper _mapper;

        public UsuariosController(db_a977c3_sumincogarContext context, UserManager<IdentityUser> userManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
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
                Expiracion = expiracion
            };
        }
        
    }
}
