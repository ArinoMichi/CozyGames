namespace CozyGames.Models
{
    public class RatingsPerfil
    {
        public RatingDetallado UltimoRating {  get; set; }
        public RatingDetallado RatingMasVotado { get; set; }
        public RatingDetallado RatingMasAlto { get; set; }
        public RatingDetallado RatingMasBajo { get; set; }
    }
}
