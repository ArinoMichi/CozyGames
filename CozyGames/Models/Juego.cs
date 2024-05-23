using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozyGames.Models
{
    [Table("JUEGODETALLES")]
    public class Juego
    {
        [Key]
        [Column("IDJUEGO")]
        public int IdJuego {  get; set; }
        [Column("TITULO")]
        public string Nombre { get; set; }
        [Column("DESCRIPCION")]
        public string Descripcion { get; set; }
        [Column("IMAGENBANNER")]
        public string ImagenBanner {  get; set; }
        [Column("IMAGENV")]
        public string ImagenVertical { get; set; }
        [Column("IMAGENH")]
        public string ImagenHorizontal { get; set; }
        [Column("ENLACE")]
        public string Enlace { get; set; }
        [Column("NOTA_PROMEDIO")]
        public decimal NotaMedia { get; set; }

    }
}
