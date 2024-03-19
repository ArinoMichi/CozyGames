using CozyGames.Extensions;
using CozyGames.Helpers;
using CozyGames.Models;
using CozyGames.Repositories;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace CozyGames.Controllers
{
    public class UsersController : Controller
    {
        private RepositoryUsers repo;
        private RepositoryGamesSqlServer repoJuegos;
        private HelperPathProvider helperPathProvider;

        public UsersController(RepositoryUsers repo, RepositoryGamesSqlServer repositoryGamesSqlServer, HelperPathProvider helperPathProvider)
        {
            this.repo = repo;
            this.repoJuegos = repositoryGamesSqlServer;
            this.helperPathProvider = helperPathProvider;
        }

        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(string email, string password)
        {
            Usuario usuario = await this.repo.LogInUserAsync(email, password);
            if (usuario == null)
            {
                ViewData["NOEXISTE"] = "El email o la contraseña son incorrectos";
                return View();

            }
            else
            {
                HttpContext.Session.SetObject("USUARIO", usuario);
                return RedirectToAction("Index", "Juegos");
            }

        }


        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(Usuario usuario)
        {
            await this.repo.RegisterUser(usuario);
            return RedirectToAction("LogIn");
        }

        public async Task<IActionResult> PanelUser(int idUser)
        {
            
            List<RatingDetallado> ratings = await this.repoJuegos.GetRatingsUserAsync(idUser);
            ViewData["RATINGS"] = ratings;
            RatingsPerfil ratingPerfil = await this.repoJuegos.GetRatingsPerfil(idUser);
            ViewData["RATINGPERFIL"] = ratingPerfil;
            ViewData["USUARIO"] = await this.repo.GetUsuarioAsync(idUser);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PanelUser(IFormFile foto, int IdUsuario, string Nombre, string Email,string PasswordOriginal)
        {
            string fileName = "img" + IdUsuario + ".jpeg";
            string path = this.helperPathProvider.MapPath(fileName, Folders.Images);
            Console.WriteLine(path);
            using(Stream stream = new FileStream(path, FileMode.Create))
            {
                await foto.CopyToAsync(stream);
            }

            Usuario perfilActualizado = new Usuario
            {
                IdUsuario = IdUsuario,
                Nombre = Nombre,
                Email = Email,
                Foto =fileName,
                PasswordOriginal = PasswordOriginal
            };
                await this.repo.UpdateProfileAsync(perfilActualizado);

                return RedirectToAction("PanelUser", new{ idUser= perfilActualizado.IdUsuario });

        }
        public async Task<IActionResult> _RatingsUser(int iduser)
        {
            ViewData["USUARIO"] = await this.repo.GetUsuarioAsync(iduser);
            List<RatingDetallado> ratings = await this.repoJuegos.GetRatingsUserAsync(iduser);
            return PartialView("_RatingsUser", ratings);
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.SetObject("USUARIO",null);
            return RedirectToAction("Index", "Juegos");
        }

        public async Task<IActionResult> DeleteComment(int idcomment, int iduser)
        {
            await this.repo.DeleteCommentAsync(idcomment);
            return RedirectToAction("_RatingsUser", new { iduser = iduser });
        }
    }
}

