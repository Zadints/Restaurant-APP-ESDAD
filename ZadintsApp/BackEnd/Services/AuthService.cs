using App.BackEnd.Helpers;
using App.Services.Ventas;
using System.Net.Mail;

namespace App.BackEnd.Services
{
    public class AuthService
    {
        #region Métodos primarios de acceso
        public static async Task<string?> Login(string userEmail, string password)
        {
            if (string.IsNullOrWhiteSpace(userEmail) || string.IsNullOrWhiteSpace(password))
                return "Ingresa todos los datos";

            if (!IsValidEmail(userEmail))
                return "El correo electrónico no es válido";

            string? contraseña = await DatabaseService.BuscarPorCorreo(userEmail);

            if (string.IsNullOrWhiteSpace(contraseña))
                return "El usuario no está registrado";

            if (!ContraseñasIguales(password, contraseña))
                return "La contraseña o correo es incorrecta";

            UsuarioService._contraseñaUser = contraseña;
            UsuarioService._usuarioActual.Correo = userEmail;

            await Task.WhenAll(
                RolService.CargarRoles(userEmail),
                UsuarioService.ObtenerImagenPerfilUsuario(),
                InventarioService.CargarProductos(userEmail),
                MaquinasServices.EstablecerConectado(),
                VentasService.CargarTicketActual()
            );

            return null;
        }

        public static async Task<string?> Register(string userEmail, string password, string confirmPassword)
        {


            if (string.IsNullOrWhiteSpace(userEmail) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                return "Ingresa todos los datos";
            }

            if (password != confirmPassword)
            {
                return "Las contraseñas no coinciden";
            }

            string? responsePassword = IsValidPassword(password);

            if (responsePassword != null)
            {
                return responsePassword;
            }

            if (!IsValidEmail(userEmail))
            {
                return "El correo electrónico no es válido";
            }

            if (DatabaseService.ExisteCorreoMySQL(userEmail))
            {
                return "El correo electrónico ya está registrado";
            }

            
            bool logroInsertar = DatabaseService.InsertarUsuarioMySQL("User", userEmail, UsuarioService._usuarioActual.Image, Encriptar(password));

            if (!logroInsertar) return "Error al registrar el usuario";

            UsuarioService._usuarioActual.Correo = userEmail;
            await Task.WhenAll(
                RolService.CargarRoles(userEmail),           
                UsuarioService.ObtenerImagenPerfilUsuario(),
                InventarioService.CargarProductos(userEmail),
                MaquinasServices.EstablecerConectado()
            );
            return null;
        }

        #endregion

        #region métodos adicionales de acceso
        public static string Encriptar(string text)
        {
            return BCrypt.Net.BCrypt.HashPassword(text);
        }

        public static bool ContraseñasIguales(string text, string encryptText)
        {
            return BCrypt.Net.BCrypt.Verify(text, encryptText);
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address.Equals(email, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        public static string? IsValidPassword(string password)
        {
            if (password.Length <= 8)
            {
                return "La contraseña debe tener al menos 8 caracteres";
            }

            if (!password.Any(char.IsUpper) && !password.Any(char.IsDigit) && !password.Any(char.IsLower))
            {
                return "La contraseña no contiene todos los tipos de caracteres requeridos";
            }

            return null;

        }
        #endregion
    }
}
