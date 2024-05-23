using CozyGames.Data;
using CozyGames.Helpers;
using CozyGames.Models;
using Microsoft.EntityFrameworkCore;

namespace CozyGames.Repositories
{
    public class RepositoryUsers
    {
        private GamesContext context;

        public RepositoryUsers(GamesContext context)
        {
            this.context = context;
        }
        public async Task<Usuario> LogInUserAsync(string email, string password)
        {
            Usuario user = await
                this.context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return null;
            }
            else
            {
                string salt = user.Salt;
                byte[] temp =
                    HelperCryptography.EncryptPassword(password, salt);
                byte[] passUser = user.Password;
                bool response =
                    HelperCryptography.CompareArrays(temp, passUser);
                if (response == true)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task RegisterUser(Usuario user)
        {

            user.IdUsuario = await this.GetMaxIdUsuarioAsync();
            //CADA USUARIO TENDRA UN SALT DISTINTO
            user.Salt = HelperCryptography.GenerateSalt();
            //GUARDAMOS EL PASSWORD EN BYTE[]
            user.PasswordOriginal = user.PasswordOriginal;
            user.Password =
                HelperCryptography.EncryptPassword(user.PasswordOriginal, user.Salt);
            user.Foto = "default.png";
            this.context.Usuarios.Add(user);
            await this.context.SaveChangesAsync();
        }
        private async Task<int> GetMaxIdUsuarioAsync()
        {
            if (this.context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await
                    this.context.Usuarios.MaxAsync(z => z.IdUsuario) + 1;
            }
        }

        public async Task<Usuario> GetUsuarioAsync(int idUser)
        {
            return await this.context.Usuarios.FirstOrDefaultAsync(z => z.IdUsuario == idUser);
        }

        public async Task DeleteCommentAsync(int idComment)
        {
            var commentToDelete = await this.context.Ratings.FindAsync(idComment);

            if (commentToDelete != null)
            {
                List<LikeComentario> likes = await this.context.LikeComentarios
                    .Where(l => l.IdRating == idComment)
                    .ToListAsync();
                this.context.LikeComentarios.RemoveRange(likes);
                await this.context.SaveChangesAsync();
                // Eliminar el comentario
                this.context.Ratings.Remove(commentToDelete);
                await this.context.SaveChangesAsync();
            }
        }
        public async Task UpdateProfileAsync(Usuario perfil)
        {
            // Buscar el perfil en la base de datos
            var perfilEnBD = await this.GetUsuarioAsync(perfil.IdUsuario);

            if (perfilEnBD != null)
            {
                // Actualizar la información del perfil
                perfilEnBD.Nombre = perfil.Nombre;
                perfilEnBD.Foto = perfil.Foto;
                perfilEnBD.Email = perfil.Email;
                //CADA USUARIO TENDRA UN SALT DISTINTO
                perfilEnBD.Salt = HelperCryptography.GenerateSalt();
                //GUARDAMOS EL PASSWORD EN BYTE[]
                perfilEnBD.PasswordOriginal = perfil.PasswordOriginal;
                perfilEnBD.Password =
                    HelperCryptography.EncryptPassword(perfil.PasswordOriginal, perfilEnBD.Salt);

                // Guardar los cambios en la base de datos
                await context.SaveChangesAsync();
            }
            else
            {
                // Manejar el caso en el que el perfil no se encuentre en la base de datos
                // Esto podría ser un error o una operación no permitida, según tu lógica de aplicación
                throw new InvalidOperationException("Perfil no encontrado en la base de datos.");
            }
        }
    }
}
