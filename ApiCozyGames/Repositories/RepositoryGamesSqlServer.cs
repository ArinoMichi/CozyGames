using ApiCozyGames.Data;
using ApiCozyGames.Helpers;
using ApiCozyGames.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApiCozyGames.Repositories
{
    public class RepositoryGamesSqlServer
    {
        private GamesContext context;

        public RepositoryGamesSqlServer(GamesContext context)
        {
            this.context = context;
        }


        public async Task<List<Juego>> GetJuegosAsync()
        {
            var consulta = from datos in this.context.Juegos
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task<Juego> GetDetalleJuego(int idJuego)
        {
            var consulta = from datos in this.context.Juegos
                           where datos.IdJuego == idJuego
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }

        public async Task<List<RatingDetallado>> GetRatingJuego(int idJuego)
        {
            var consulta = from datos in this.context.RatingDetallados
                           where datos.IdJuego == idJuego
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task<List<RatingDetallado>> GetRatingsUserAsync(int idUsuario)
        {
            return await this.context.RatingDetallados
                .Where(o => o.IdUsuario == idUsuario)
                .ToListAsync();
        }
        public async Task CreateRating(Rating rating)
        {
            int nuevoId = await
                    this.context.Ratings.MaxAsync(z => z.IdRating) + 1;
            rating.IdRating = nuevoId;
            DateTime fecha = DateTime.Now;
            rating.Fecha = fecha;
            this.context.Ratings.Add(rating);
            await this.context.SaveChangesAsync();
        }


        public async Task<List<Juego>> GetTop5JuegosAsync()
        {
            var top5 = await this.context.Juegos
                                .OrderByDescending(j => j.NotaMedia)
                                .Take(5)
                                .ToListAsync();
            return top5;

        }

        public async Task<List<RatingDetallado>> GetTopCommentsAsync()
        {
            var top4 = await this.context.RatingDetallados
                                .OrderByDescending(z => z.LikesTotales)
                                .Take(4)
                                .ToListAsync();
            return top4;
        }

        public async Task<RatingsPerfil> GetRatingsPerfil(int idUser)
        {
            var ratingsUser = await GetRatingsUserAsync(idUser);

            if (ratingsUser == null || ratingsUser.Count == 0)
            {
                return null;
            }

            var ultimoRating = ratingsUser.OrderByDescending(r => r.Fecha).FirstOrDefault();
            var ratingMasVotado = ratingsUser.OrderByDescending(r => r.LikesTotales).FirstOrDefault();
            var ratingMasAlto = ratingsUser.OrderByDescending(r => r.Nota).FirstOrDefault();
            var ratingMasBajo = ratingsUser.OrderBy(r => r.Nota).FirstOrDefault();

            RatingsPerfil perfil = new RatingsPerfil
            {
                UltimoRating = ultimoRating,
                RatingMasVotado = ratingMasVotado,
                RatingMasAlto = ratingMasAlto,
                RatingMasBajo = ratingMasBajo
            };

            return perfil;
        }

        public async Task LikeRating(int idUser, int idRating)
        {
            int nuevoId = 0;
            if (this.context.LikeComentarios.Count() == 0) nuevoId = 1;
            else nuevoId = this.context.LikeComentarios.Max(r => r.IdLike) + 1;
            LikeComentario like = new LikeComentario
            {
                IdLike = nuevoId,
                IdRating = idRating,
                IdUsuario = idUser
            };
            this.context.LikeComentarios.Add(like);
            await this.context.SaveChangesAsync();
        }


        public async Task<List<LikeComentario>> GetComentariosUserAsync(int idUser)
        {
            return await this.context.LikeComentarios.Where(x => x.IdUsuario == idUser).ToListAsync();
        }

        public async Task<List<Juego>> FilterJuegosAsync(string cadena)
        {
            List<Juego> juegos = await GetJuegosAsync();
            return juegos.Where(j => j.Nombre.Contains(cadena, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        public async Task UpdateLikeComentarioAsync(int ratingid, int idusuario)
        {
            LikeComentario like = await this.context.LikeComentarios.FirstOrDefaultAsync(l => l.IdRating == ratingid && l.IdUsuario == idusuario);
            if (like == null)
            {
                await LikeRating(idusuario, ratingid);
            }
            else
            {
                this.context.LikeComentarios.Remove(like);
            }
            await this.context.SaveChangesAsync();
        }
        public async Task<bool> DeleteCommentAsync(int idUser,int idComment)
        {
            var commentToDelete = await this.context.Ratings.FindAsync(idComment);

            if (commentToDelete != null)
            {
                if(commentToDelete.IdUsuario != idUser)
                {
                    return false;
                }
                else
                {
                    List<LikeComentario> likes = await this.context.LikeComentarios
                                       .Where(l => l.IdRating == idComment)
                                       .ToListAsync();
                    this.context.LikeComentarios.RemoveRange(likes);
                    await this.context.SaveChangesAsync();
                    // Eliminar el comentario
                    this.context.Ratings.Remove(commentToDelete);
                    await this.context.SaveChangesAsync();
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }

}