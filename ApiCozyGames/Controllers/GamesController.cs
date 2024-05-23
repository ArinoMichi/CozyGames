using ApiCozyGames.Models;
using ApiCozyGames.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiCozyGames.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private RepositoryGamesSqlServer repo;

        public GamesController(RepositoryGamesSqlServer repo)
        {
            this.repo = repo;
        }
        [HttpGet("GetJuegos")]
        public async Task<ActionResult<List<Juego>>> GetJuegos()
        {
           return await this.repo.GetJuegosAsync();
        }
        [HttpGet("GetDetalleJuego/{id}")]
        public async Task<ActionResult<Juego>> GetDetalleJuego(int id)
        {
            return await this.repo.GetDetalleJuego(id);
        }
        [HttpGet]
        [Route("GetTopJuegos")]
        public async Task<ActionResult<List<Juego>>> GetTop5Juegos()
        {
            return await this.repo.GetTop5JuegosAsync();
        }
        [HttpGet("FilterGames/{cadena}")]
        public async Task<ActionResult<List<Juego>>> FilterJuegos(string cadena)
        {
            return await this.repo.FilterJuegosAsync(cadena);
        }



    }
}
