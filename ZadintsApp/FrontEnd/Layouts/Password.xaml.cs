using App.BackEnd.Domain.Entities;
using App.BackEnd.Domain.Nodo;
using App.BackEnd.Services;
using App.Components.windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace App.Components.Layouts.Body
{
    /// <summary>
    /// Lógica de interacción para RolesGui.xaml
    /// </summary>
    public partial class Password : Page
    {
        private string _nombreRol;
        public Password(string NombreRol)
        {
            InitializeComponent();
            _nombreRol = NombreRol;
        }


        private void btnCambiarRol_Click(object sender, RoutedEventArgs e)
        {
            string contraseñaRol = tbxPasswordRol.Text;

            if (AuthService.ContraseñasIguales(contraseñaRol, UsuarioService._contraseñaUser))
            {
                accion();
            }

            NodoSimple<Rol> actual = RolService._role.Cabeza;
            while (actual != null)
            {
                if (actual.Dato.Nombre == _nombreRol)
                {
                    

                    if (actual.Dato.Contraseña == contraseñaRol)
                    {
                        accion();
                    }
                    else
                    {

                        MessageBox.Show("Contraseña incorrecta");
                        Dashboard mainWindow = (Dashboard)Application.Current.MainWindow;

                        mainWindow.frBody.Content = null;
                        mainWindow.frBody.Visibility = Visibility.Collapsed;
                        return;
                    }
                }
                actual = actual.Siguiente;
            }
        }

        private void accion()
        {
            RolService._cambiarRol = true;

            Dashboard mainWindow = (Dashboard)Application.Current.MainWindow;

            mainWindow.frBody.Content = null;
            mainWindow.frBody.Visibility = Visibility.Collapsed;

            return;
        }

        private void btnCloseRoleGui_Click(object sender, RoutedEventArgs e)
        {
            RolService._cambiarRol = false;

            Dashboard mainWindow = (Dashboard)Application.Current.MainWindow;

            mainWindow.frBody.Content = null;
            mainWindow.frBody.Visibility = Visibility.Collapsed;

            return;
        }
    }
}
