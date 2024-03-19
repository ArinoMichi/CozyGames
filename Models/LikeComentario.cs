using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozyGames.Models
{
    [Table("LIKE_COMENTARIO")]
    public class LikeComentario
    {
        [Key]
        [Column("IDLIKE")]
        public int IdLike { get; set; }
        [Column("IDRATING")]
        public int IdRating { get; set; }
        [Column("IDUSUARIO")]
        public int IdUsuario { get; set; }
    }
}

