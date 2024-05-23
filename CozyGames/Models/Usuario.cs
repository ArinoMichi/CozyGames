
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozyGames.Models
{
    [Table("USUARIO")]
    public class Usuario
    {
        [Key]
        [Column("IDUSUARIO")]
        public int IdUsuario { get; set; }
        [Column("NOMBRE")]
        public string Nombre { get; set; }
        [Column("EMAIL")]
        public string Email { get; set; }
        [Column("FOTO")]
        public string Foto { get; set; }
        [Column("PASS")]
        public byte[] Password { get; set; }
        [Column("PASSORIGEN")]
        public string PasswordOriginal { get; set; }
        [Column("SALT")]
        public string Salt { get; set; }
    }
}
