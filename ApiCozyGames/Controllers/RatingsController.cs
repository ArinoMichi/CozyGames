using ApiCozyGames.Models;
using ApiCozyGames.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiCozyGames.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly RepositoryGamesSqlServer repo;

        public RatingsController(RepositoryGamesSqlServer repo)
        {
            this.repo = repo;
        }

        [HttpGet("GetRatingsJuego/{idJuego}")]
        public async Task<ActionResult<List<RatingDetallado>>> GetRatingsJuego(int idJuego)
        {
            return await this.repo.GetRatingJuego(idJuego);
        }

        [HttpGet("GetRatingsUsuario/{idUsuario}")]
        public async Task<ActionResult<List<RatingDetallado>>> GetRatingsUser(int idUsuario)
        {
            return await this.repo.GetRatingsUserAsync(idUsuario);
        }

        [HttpPost("CrearRating")]
        public async Task<ActionResult> CreateRating(Rating rating)
        {
            await this.repo.CreateRating(rating);
            return Ok(rating);
        }

        [HttpGet("TopRatings")]
        public async Task<ActionResult<List<RatingDetallado>>> GetTopRatings()
        {
            return await this.repo.GetTopCommentsAsync();
        }

        [HttpGet("GetRatingsPerfil/{idUsuario}")]
        public async Task<ActionResult<RatingsPerfil>> GetRatingsPerfil(int idUsuario)
        {
            return await this.repo.GetRatingsPerfil(idUsuario);
        }

        //[HttpPost("CreateLike/{idUser}/{idRating}")]
        //public async Task<ActionResult> LikeRating(int idUser, int idRating)
        //{
        //    await this.repo.LikeRating(idUser, idRating);
        //    return Ok();
        //}

        [HttpGet("GetLikesUser/{idUser}")]
        public async Task<ActionResult<List<LikeComentario>>> GetLikesUser(int idUser)
        {
            return await this.repo.GetComentariosUserAsync(idUser);
        }

        [Authorize]
        [HttpGet("UpdateLikeComentario/{idcomentario}")]
        public async Task<ActionResult> UpdateLike(int idcomentario)
        {
            string jsonUsuario = HttpContext.User.FindFirst(x => x.Type == "UserData").Value;
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(jsonUsuario);
            await this.repo.UpdateLikeComentarioAsync(idcomentario, usuario.IdUsuario);
            return Ok();
        }

        [Authorize]
        [HttpDelete("DeleteRating/{idComment}")]
        public async Task<ActionResult> DeleteComment(int idComment)
        {
            string jsonUsuario = HttpContext.User.FindFirst(x => x.Type == "UserData").Value;
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(jsonUsuario);
            // Eliminar el comentario con el ID proporcionado
            Boolean response =  await repo.DeleteCommentAsync(usuario.IdUsuario,idComment);
            if(response == true)
            {
                // Devolver una respuesta exitosa
                return Ok();
            }
            else
            {
                return Unauthorized();
            }

            
        }


    }
}
