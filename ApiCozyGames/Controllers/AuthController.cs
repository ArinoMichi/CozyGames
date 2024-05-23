using ApiCozyGames.Helpers;
using ApiCozyGames.Models;
using ApiCozyGames.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiCozyGames.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly RepositoryUsers repo;
        private readonly HelperActionServicesOAuth helper;

        public AuthController(RepositoryUsers repo, HelperActionServicesOAuth helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            Usuario user = await this.repo.LogInUsuarioAsync(model.Email, model.Password);
            if (user == null)
            {
                return Unauthorized();
            }
            else
            {
                SigningCredentials credentials =
                    new SigningCredentials(this.helper.GetKeyToken(),
                    SecurityAlgorithms.HmacSha256);
                string jsonUsuario = JsonConvert.SerializeObject(user);
                Claim[] info = new[]
                {
                    new Claim("UserData",jsonUsuario)
                };
                JwtSecurityToken token =
                    new JwtSecurityToken(
                        claims: info,
                        issuer: this.helper.Issuer,
                        audience: this.helper.Audience,
                        signingCredentials: credentials,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        notBefore: DateTime.UtcNow
                        );
                return Ok(
                    new
                    {
                        response = new JwtSecurityTokenHandler().WriteToken(token)
                    });
            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            // Crear un objeto Usuario a partir del modelo de registro
            var user = new Usuario
            {
                Nombre = model.Nombre,
                Email = model.Email,
                PasswordOriginal = model.Password,
                // Otros campos del usuario si los tienes
            };

            // Registrar el usuario en la base de datos
            await repo.RegisterUser(user);

            // Devolver una respuesta exitosa
            return Ok();
        }

        [HttpGet("GetProfile/{idUser}")]
        public async Task<ActionResult<Usuario>> GetProfile(int idUser)
        {
            // Obtener el perfil del usuario con el ID proporcionado
            var perfil = await repo.GetUsuarioAsync(idUser);
            //alta seguridad!!!!
            perfil.Password = new byte[] { };
            perfil.Salt = "cifrao";
            perfil.PasswordOriginal = "cifrao";


            // Si el perfil existe, devolverlo como resultado
            if (perfil != null)
            {
                return Ok(perfil);
            }
            // Si el perfil no existe, devolver un error 404
            else
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> UpdateProfile(string foto, string nombre, string email, string password)
        {
            string json = HttpContext.User.FindFirst(x => x.Type == "UserData").Value;
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(json);
            usuario.Foto = foto;
            usuario.Nombre = nombre;
            usuario.Email = email;
            usuario.PasswordOriginal = password;

            // Actualizar el perfil del usuario
            await repo.UpdateProfileAsync(usuario);

            // Devolver una respuesta exitosa
            return Ok();
        }

        [Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult<Usuario>> GetLoggedUser()
        {
            string json = HttpContext.User.FindFirst(x => x.Type == "UserData").Value;
            Usuario user = JsonConvert.DeserializeObject<Usuario>(json);
            Usuario currentUser = await this.repo.GetUsuarioAsync(user.IdUsuario);
            return currentUser;
        }
    }
}
