using App.BackEnd.Domain.Entities;
using App.BackEnd.Helpers;

namespace App.BackEnd.Services
{
    internal class UsuarioService
    {
        public static UsuarioEntity _usuarioActual { get; set; } = new UsuarioEntity();
        public static string _contraseñaUser { get; set; }
        internal static async Task GuardarImagenPerfilUsuario(string ruta)
        {
            string correo = _usuarioActual.Correo;
            if (string.IsNullOrWhiteSpace(correo)) throw new Exception("No se estableció bien el perfil al cargar la aplicación");
            string base64 = Imagen.ConvertirABase64(ruta);
            await DatabaseService.ActualizarImgUsuarioMySQL(correo, base64);
            _usuarioActual.Image = base64;
        }

        internal static async Task ObtenerImagenPerfilUsuario()
        {
            string correo = _usuarioActual.Correo;
            if (string.IsNullOrWhiteSpace(correo)) throw new Exception("No se estableció bien el perfil al cargar la aplicación");

            string? imagenBase64 = await DatabaseService.ObtenerImgUsuarioMySQL(correo);

            if (imagenBase64 != null)
            {
                _usuarioActual.Image = imagenBase64;
            }

            
        }
    }
}
