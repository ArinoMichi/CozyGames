using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozyGames.Models
{
    [Table("V_USER_GAME_RATING")]
    public class RatingDetallado
    {
        [Key]
        [Column("IDRATING")]
        public int IdRating { get; set; }
        [Column("ID_JUEGO")]
        public int IdJuego { get; set; }
        [Column("NOMBRE_JUEGO")]
        public string NombreJuego { get; set; }
        [Column("FOTO_JUEGO")]
        public string FotoJuego { get; set; }
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }
        [Column("NOMBRE_USUARIO")]
        public string NombreUser { get; set; }
        [Column("FOTO_USUARIO")]
        public string FotoUser { get; set; }
        [Column("NOTA")]
        public decimal Nota { get; set; }
        [Column("TITULO")]
        public string Titulo { get; set; }
        [Column("COMENTARIO")]
        public string Comentario { get; set; }
        [Column("FECHA")]
        public DateTime Fecha { get; set; }
        [Column("LIKES_TOTALES")]
        public int LikesTotales { get; set; }

    }
}