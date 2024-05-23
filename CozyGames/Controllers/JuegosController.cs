using CozyGames.Extensions;
using CozyGames.Models;
using CozyGames.Repositories;
using CozyGames.Services;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace CozyGames.Controllers
{
    public class JuegosController : Controller
    {
        //private RepositoryGamesSqlServer repo;
        private ServiceGames service;

        public JuegosController(ServiceGames service)
        {
            this.service = service;
        }
        public async Task<IActionResult> Index()
        {
            List<Juego> juegos = await this.service.GetJuegosAsync();
            ViewData["TOP10"] = await this.service.GetTop5JuegosAsync();
            ViewData["TOPRAT"] = await this.service.GetTopRatingsAsync();
            return View(juegos);
        }
        public async Task<IActionResult> Details(int id)
        {
            Usuario user = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string token = HttpContext.User.FindFirst(x => x.Type == "TOKEN").Value;
                user = await service.GetCurentUser(token);
            }
            ViewData["USER"] = user;
            Juego juego = await this.service.GetDetalleJuegoAsync(id);
            List<RatingDetallado> ratings = await this.service.GetRatingsJuegoAsync(id);
            ViewData["RATINGS"] = ratings;
            return View(juego);
        }

        public async Task<IActionResult> _Ratings(int id)
        {
            List<RatingDetallado> ratings = await this.service.GetRatingsJuegoAsync(id);
            return PartialView("_Ratings", ratings);
        }

        public async Task<IActionResult> DarLike(int commentid, int idjuego)
        {
            await this.service.UpdateLikeComentario(commentid);
            return RedirectToAction("_Ratings", "Juegos", new { id = idjuego });
        }

        public async Task<IActionResult> Rate(int idjuego)
        {
            Juego juego = await this.service.GetDetalleJuegoAsync(idjuego);
            return View(juego);
        }

        [HttpPost]
        public async Task<IActionResult> Rate(Rating rating)
        {
            Usuario user = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string token = HttpContext.User.FindFirst(x => x.Type == "TOKEN").Value;
                user = await service.GetCurentUser(token);
            }
            rating.IdUsuario = user.IdUsuario;
            await this.service.CreateRatingAsync(rating);
            return RedirectToAction("Details", new { id = rating.IdJuego });
        }
        public async Task<IActionResult> ListaJuegos()
        {
            List<Juego> juegos = await this.service.GetJuegosAsync();

            return View(juegos);
        }

        [HttpPost]
        public async Task<IActionResult> ListaJuegos(string? search)
        {
            List<Juego> juegos;
            if (search == null)
            {
                juegos = await this.service.GetJuegosAsync();
            }
            else
            {
                juegos = await this.service.FilterJuegosAsync(search);
            }
            return View(juegos);
        }

    }
}
