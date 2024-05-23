using CozyGames.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CozyGames.Services
{
    public class ServiceGames
    {
        private string UrlApi;
        private MediaTypeWithQualityHeaderValue header;
        private IHttpContextAccessor httpContextAccessor;

        public ServiceGames(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.header =
                new MediaTypeWithQualityHeaderValue("application/json");
            this.UrlApi = configuration.GetValue<string>("ApiUrls:ApiCozyGames");
            this.httpContextAccessor = httpContextAccessor;
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                HttpResponseMessage response = await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        private async Task<T> CallApiAsync<T>(string request, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                HttpResponseMessage response = await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }
        #region AUTH
        public async Task<string> LoginAsync(string email, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/Auth/Login";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                string json = JsonConvert.SerializeObject(new
                {
                    Email = email,
                    Password = password
                });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(request, content);
                string data = await response.Content.ReadAsStringAsync();
                JObject keys = JObject.Parse(data);
                string token = keys.GetValue("response").ToString();
                return token;
            }
        }

        public async Task RegisterUserAsync(string nombre, string email, string password, string foto)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/Auth/Register";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                string json = JsonConvert.SerializeObject(new
                {
                    Nombre = nombre,
                    Email = email,
                    Password = password,
                    Foto = foto
                });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(request, content);

            }
        }
        public async Task<Usuario> GetUsuarioAsync(int idUser)
        {
            string request = "api/Auth/GetProfile/" + idUser;
            Usuario user = await this.CallApiAsync<Usuario>(request);
            return user;
        }


        public async Task UpdateUserAsync(Usuario perfil)
        {
            using (HttpClient client = new HttpClient())
            {
                // Obtener el token de la sesión
                string token = httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "TOKEN").Value;

                // Construir la URL con los parámetros de consulta
                string request = $"api/Auth/UpdateProfile?foto={perfil.Foto}&nombre={perfil.Nombre}&email={perfil.Email}&password={perfil.PasswordOriginal}";

                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                // Agregar el token al encabezado de autorización
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                // Realizar la solicitud utilizando el método GET
                HttpResponseMessage response = await client.PutAsync(request, null);
                // Manejar la respuesta según sea necesario
            }
        }



        public async Task<Usuario> GetCurentUser(string token)
        {
            string request = "api/Auth/GetLoggedUser/";
            Usuario user = await this.CallApiAsync<Usuario>(request, token);
            return user;
        }

        #endregion



        #region JUEGOS
        public async Task<List<Juego>> GetJuegosAsync()
        {
            string request = "api/Games/GetJuegos";
            List<Juego> juegos = await this.CallApiAsync<List<Juego>>(request);
            return juegos;
        }

        public async Task<Juego> GetDetalleJuegoAsync(int idJuego)
        {
            string request = "api/Games/GetDetalleJuego/" + idJuego;
            Juego juego = await this.CallApiAsync<Juego>(request);
            return juego;
        }

        public async Task<List<Juego>> GetTop5JuegosAsync()
        {
            string request = "api/Games/GetTopJuegos";
            List<Juego> juegos = await this.CallApiAsync<List<Juego>>(request);
            return juegos;
        }
        public async Task<List<Juego>> FilterJuegosAsync(string cadena)
        {
            string request = "api/Games/FilterGames/" + cadena;
            List<Juego> juegos = await this.CallApiAsync<List<Juego>>(request);
            return juegos;
        }
        #endregion
        #region RATINGS
        public async Task<List<RatingDetallado>> GetRatingsJuegoAsync(int idJuego)
        {
            string request = "api/Ratings/GetRatingsJuego/" + idJuego;
            List<RatingDetallado> ratings = await this.CallApiAsync<List<RatingDetallado>>(request);
            return ratings;
        }

        public async Task<List<RatingDetallado>> GetRatingsUsuarioAsync(int idUsuario)
        {
            string request = "api/Ratings/GetRatingsUsuario/" + idUsuario;
            List<RatingDetallado> ratings = await this.CallApiAsync<List<RatingDetallado>>(request);
            return ratings;
        }

        public async Task CreateRatingAsync(Rating rating)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/Ratings/CrearRating";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                string json = JsonConvert.SerializeObject(rating);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(request, content);
            }
        }

        public async Task<List<RatingDetallado>> GetTopRatingsAsync()
        {
            string request = "api/Ratings/TopRatings";
            List<RatingDetallado> ratings = await this.CallApiAsync<List<RatingDetallado>>(request);
            return ratings;
        }
        public async Task<RatingsPerfil> GetRatingsPerfilAsync(int idUser)
        {
            string request = "api/Ratings/GetRatingsPerfil/" + idUser;
            RatingsPerfil ratings = await this.CallApiAsync<RatingsPerfil>(request);
            return ratings;
        }


        public async Task<List<LikeComentario>> GetLikesUserAsync(int idUser)
        {
            string request = "api/Ratings/GetLikesUser/" + idUser;
            List<LikeComentario> likes = await this.CallApiAsync<List<LikeComentario>>(request);
            return likes;
        }
        public async Task UpdateLikeComentario(int idComentario)
        {
            string token = httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/Ratings/UpdateLikeComentario/" + idComentario;
            await this.CallApiAsync<string>(request, token);
        }

        public async Task DeleteRatingAsync(int idRating)
        {
            using (HttpClient client = new HttpClient())
            {
                string token = httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "TOKEN").Value;
                string request = "api/Ratings/DeleteRating/" + idRating;
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                HttpResponseMessage response =
                    await client.DeleteAsync(request);
            }
        }
        #endregion

    }
}
