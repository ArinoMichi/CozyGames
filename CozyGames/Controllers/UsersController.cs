using CozyGames.Extensions;
using CozyGames.Helpers;
using CozyGames.Models;
using CozyGames.Repositories;
using CozyGames.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Security.Claims;

namespace CozyGames.Controllers
{
    public class UsersController : Controller
    {
        //private RepositoryUsers repo;
        //private RepositoryGamesSqlServer repoJuegos;
        private ServiceGames service;
        private HelperPathProvider helperPathProvider;
        private ServiceStorageBlobs serviceBlobs;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public UsersController(ServiceGames service, HelperPathProvider helperPathProvider, ServiceStorageBlobs serviceBlobs, IWebHostEnvironment hostingEnvironment)
        {
            this.service = service;
            this.helperPathProvider = helperPathProvider;
            this.serviceBlobs = serviceBlobs;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(string email, string password)
        {
            string token = await this.service.LoginAsync(email, password);
            if (token == null)
            {
                ViewData["NOEXISTE"] = "El email o la contraseña son incorrectos";
                return View();

            }
            else
            {
                Usuario user = await this.service.GetCurentUser(token);
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Email));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.IdUsuario.ToString()));
                identity.AddClaim(new Claim("TOKEN", token));

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync
                    (CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

                return RedirectToAction("Index", "Juegos");
            }
        }


        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(int IdUsuario, string Nombre, string Email, string PasswordOriginal)
        {
            string defaultFileName = "default.png";
            string defaultImagePath = Path.Combine(_hostingEnvironment.WebRootPath, defaultFileName);

            if (System.IO.File.Exists(defaultImagePath))
            {
                string fileName = "img" + IdUsuario + ".jpeg";
                
                using (Stream stream = System.IO.File.OpenRead(defaultImagePath))
                {
                    await this.serviceBlobs.UploadBlobAsync("fotos", fileName, stream);
                }
                Usuario nuevoUsuario = new Usuario
                {
                    IdUsuario = IdUsuario,
                    Nombre = Nombre,
                    Email = Email,
                    Foto = fileName,
                    PasswordOriginal = PasswordOriginal
                };

                await this.service.RegisterUserAsync(nuevoUsuario.Nombre, nuevoUsuario.Email, nuevoUsuario.PasswordOriginal, nuevoUsuario.Foto);

                return RedirectToAction("LogIn");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public async Task<IActionResult> PanelUser(int idUser)
        {

            List<RatingDetallado> ratings = await this.service.GetRatingsUsuarioAsync(idUser);
            ViewData["RATINGS"] = ratings;
            RatingsPerfil ratingPerfil = await this.service.GetRatingsPerfilAsync(idUser);
            ViewData["RATINGPERFIL"] = ratingPerfil;
            ViewData["USUARIO"] = await this.service.GetUsuarioAsync(idUser);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PanelUser(IFormFile foto, int IdUsuario, string Nombre, string Email, string PasswordOriginal)
        {
            string fileName = "img" + IdUsuario + ".jpeg";
            string path = this.helperPathProvider.MapPath(fileName, Folders.Images);
            
            using (Stream stream = foto.OpenReadStream())
            {
                await this.serviceBlobs.UploadBlobAsync("fotos", fileName, stream);
            }

            Usuario perfilActualizado = new Usuario
            {
                IdUsuario = IdUsuario,
                Nombre = Nombre,
                Email = Email,
                Foto = fileName,
                PasswordOriginal = PasswordOriginal
            };
            await this.service.UpdateUserAsync(perfilActualizado);

            return RedirectToAction("PanelUser", new { idUser = perfilActualizado.IdUsuario });

        }
        public async Task<IActionResult> _RatingsUser(int iduser)
        {
            ViewData["USUARIO"] = await this.service.GetUsuarioAsync(iduser);
            List<RatingDetallado> ratings = await this.service.GetRatingsUsuarioAsync(iduser);
            return PartialView("_RatingsUser", ratings);
        }

        public async Task<IActionResult> LogOutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Juegos");
        }

        public async Task<IActionResult> DeleteComment(int idcomment, int iduser)
        {
            await this.service.DeleteRatingAsync(idcomment);
            return RedirectToAction("_RatingsUser", new { iduser = iduser });
        }
    }
}

