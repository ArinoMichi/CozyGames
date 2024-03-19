using CozyGames.Extensions;
using CozyGames.Models;
using CozyGames.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CozyGames.Controllers
{
    public class JuegosController : Controller
    {
        private RepositoryGamesSqlServer repo;

        public JuegosController(RepositoryGamesSqlServer repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<Juego> juegos = await this.repo.GetJuegosAsync();
            ViewData["TOP10"] = await this.repo.GetTop5JuegosAsync();
            ViewData["TOPRAT"] = await this.repo.GetTopCommentsAsync();
            return View(juegos);
        }
        public async Task<IActionResult> Details(int id)
        {
            Usuario user = HttpContext.Session.GetObject<Usuario>("USUARIO");
            Juego juego = await this.repo.GetDetalleJuego(id);
            // List<RatingDetallado> ratings = await this.repo.GetRatingJuego(id);
            // ViewData["RATINGS"] = ratings;
            return View(juego);
        }

        public async Task<IActionResult> _Ratings(int id)
        {
            List<RatingDetallado> ratings = await this.repo.GetRatingJuego(id);
            return PartialView("_Ratings", ratings);
        }

        public async Task<IActionResult> DarLike(int commentid, int idjuego)
        {
            int idusuario = HttpContext.Session.GetObject<Usuario>("USUARIO").IdUsuario;
            await this.repo.UpdateLikeComentarioAsync(commentid, idusuario);
            return RedirectToAction("_Ratings", "Juegos", new { id = idjuego });
        }

        public async Task<IActionResult> Rate(int idjuego)
        {
            Juego juego = await this.repo.GetDetalleJuego(idjuego);
            return View(juego);
        }

        [HttpPost]
        public async Task<IActionResult> Rate(Rating rating)
        {
            rating.IdUsuario = HttpContext.Session.GetObject<Usuario>("USUARIO").IdUsuario;

            await this.repo.CreateRating(rating);
            return RedirectToAction("Details", new { id = rating.IdJuego });
        }
        public async Task<IActionResult> ListaJuegos()
        {
            List<Juego> juegos= await this.repo.GetJuegosAsync();
            
            return View(juegos);
        }

        [HttpPost]
        public async Task<IActionResult> ListaJuegos(string? search)
        {
            List<Juego> juegos;
            if (search == null)
            {
                juegos = await this.repo.GetJuegosAsync();
            }
            else
            {
                juegos = await this.repo.FilterJuegosAsync(search);
            }
            return View(juegos);
        }

    }
}
