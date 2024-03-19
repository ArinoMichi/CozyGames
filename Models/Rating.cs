using CozyGames.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozyGames.Models
{
    [Table("RATING")]
    public class Rating
    {
        [Key]
        [Column("IDRATING")]
        public int IdRating { get; set; }
        [Column("IDJUEGO")]
        public int IdJuego { get; set; }
        [Column("IDUSUARIO")]
        public int IdUsuario { get; set; }
        [Column("NOTA")]
        public decimal Nota { get; set; }
        [Column("TITULO")]
        public string Titulo { get; set; }
        [Column("COMENTARIO")]
        public string Comentario { get; set; }
        [Column("FECHA")]
        public DateTime Fecha { get; set; }
    }
}
