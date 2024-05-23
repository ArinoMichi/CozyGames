using ApiCozyGames.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ApiCozyGames.Data
{
    public class GamesContext : DbContext
    {
        public GamesContext(DbContextOptions<GamesContext> options) : base(options) { }

        public DbSet<Juego> Juegos { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<RatingDetallado> RatingDetallados { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<LikeComentario> LikeComentarios { get; set; }
    }
}
