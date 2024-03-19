using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozyGames.Models
{
    [Table("V_USER_GAME_RATING")]
    public class DetallesRating : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

//CREATE VIEW V_USER_GAME_RATING AS
//SELECT
//    R.IDRATING,
//    J.IDJUEGO AS ID_JUEGO,
//    J.TITULO AS NOMBRE_JUEGO,
//    J.IMAGEN AS IMAGEN_JUEGO,
//    U.IDUSUARIO AS ID_USUARIO,
//    U.NOMBRE AS NOMBRE_USUARIO,
//    U.FOTO AS FOTO_USUARIO,
//    R.NOTA,
//    R.COMENTARIO,
//    R.FECHA
//FROM