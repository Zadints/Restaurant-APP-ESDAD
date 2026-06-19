using App.BackEnd.Domain.Entities;
using App.BackEnd.Domain.Nodo;
using App.BackEnd.Services.ESDAD;

using System.Data;
using System.Windows;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace App.BackEnd.Services
{
    public class RolService
    {
        public static ListaSimple<Rol> _role = new ListaSimple<Rol>();
        public static bool _cambiarRol = false;

        public static async Task<string?> Insertar(string nombre, string descripcion, PermisosEntity permisos, string contraseña)
        {
            if (_role.Contar() >= 3)
            {
                return "No se pueden crear más de 3 roles en el restaurante Ayllu.";
            }

            if (string.IsNullOrWhiteSpace(nombre))
            {

                return "Por favor, ingresa un nombre para el rol.";
            }

            if (string.IsNullOrWhiteSpace(contraseña))
            {
                return "Por favor, ingresa una contraseña para el rol.";
            }

            if (string.IsNullOrWhiteSpace(descripcion))
            {
                descripcion = "Un nuevo rol para el restaurante Ayllu";
            }

          


            /*-------------------------------------------------
             * 
             * A partir de aqui entra SQl 1 donde el promer paso es buscar si
             * el buscar si el nombre del rol + correo existe en una linea de la
             * bases de datos. 
             * 
            --------------------------------------------------*/
            string correoActual = UsuarioService._usuarioActual.Correo;

            int CoincidenciasEncontradas = await DatabaseService.ContarRoles(nombre, correoActual);
            if (CoincidenciasEncontradas > 0)
            {
               return "El rol ya existe";
            }
            string permisosId = Guid.NewGuid().ToString();
            
            Rol nuevoRol = new Rol(nombre, descripcion, permisos, permisosId, correoActual, contraseña);
            _role.InsertarCabeza(nuevoRol);
            /*-------------------------------------------------
            * 
            * A partir de aqui entra a la preparació nde datos para el
            * registro de datos en las tablas.
            * 
            *
            *
            *    Tabla1:
            *        Roles:
            +        -   Nombre
            *        -   Descripción
            *        -   PermisosID (Clave para relacionar)
            *       -   Color

            *    Tabla2:
            *        Permisos:
            *            -   PermisosID (Clave para relacionar)
            *            -   Venderpr....
            *
            --------------------------------------------------*/

            _ = Task.Run(async () =>
            {
                await DatabaseService.InsertarRol(nombre, descripcion, permisosId, correoActual, contraseña);

                await DatabaseService.InsertarPermisos(
                    permisosId,
                    permisos.VenderProductos,
                    permisos.EliminarProductos,
                    permisos.AgregarProductos,
                    permisos.EditarProductos,
                    permisos.VerClientes
                );
            }); 
            
            return null;
        }

        internal static async Task CargarRoles(string correo)
        {
            DataTable dt = DatabaseService.ObtenerRolesPorCorreo(correo);

            ListaSimple<Rol> lista = new ListaSimple<Rol>();

            foreach (DataRow row in dt.Rows)
            {
                string permisosId = row["PermisosId"].ToString()!;

                PermisosEntity? permisos =
                   DatabaseService.ObtenerPermisos(permisosId);

                if (permisos == null)
                {
                    return;
                }

                lista.InsertarCabeza(
                    new Rol(
                        row["Nombre"].ToString()!,
                        row["Descripcion"].ToString()!,
                        permisos,
                        permisosId,
                        row["UserMail"].ToString()!,
                        row["Contrasena"].ToString()
                    )
                );
            }

            _role = lista;
        }



        public static string? EliminarRol(string permisosId)
        {
            bool rolEliminado = DatabaseService.EliminarRolDB(permisosId);

            if (!rolEliminado)
                return "Error al eliminar el rol";

            bool permisoEliminado = DatabaseService.EliminarPermisosDB(permisosId);

            if (!permisoEliminado)
                return "Error al eliminar el permiso";

            var predicate = new Predicate<Rol>(
                p => p.permisosId == permisosId);

            _role.Eliminar(predicate);

            return null;
        }


        public static string? CambiarUsuarioRol(string nuevoRolNombre)
        {
            if(UsuarioService._usuarioActual.RolActual != null)
            {
                if (UsuarioService._usuarioActual.RolActual.Nombre == nuevoRolNombre)
                {
                    return "El rol actual ya lo tienes seleccionado";
                }
            }

            NodoSimple<Rol> actual = _role.Cabeza;

            Rol? entidadDelNuevoRol = null;

            while (actual != null)
            {
                if (actual.Dato.Nombre.Equals(nuevoRolNombre, StringComparison.OrdinalIgnoreCase))
                {
                    entidadDelNuevoRol = actual.Dato;
                    break;
                }
                actual = actual.Siguiente;
            }

            if (entidadDelNuevoRol == null) return "No se pudo asignar el rol al usuario porque no existe.";

            UsuarioService._usuarioActual.RolActual = entidadDelNuevoRol;

            return null;
        }

    }
}

