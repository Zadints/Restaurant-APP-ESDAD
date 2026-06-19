using App.BackEnd.Domain.Entities;
using App.BackEnd.Domain.Nodo;
using App.BackEnd.Helpers;
using App.BackEnd.Services;
using App.Components.Layouts.Body;
using App.Components.windows;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Zrutas.UI.Views.Frames
{
    /// <summary>
    /// Lógica de interacción para Setting.xaml
    /// </summary>
    public partial class Setting : Page
    {
        Dashboard mainWindow = (Dashboard)Application.Current.MainWindow;

        public Setting()
        {

            InitializeComponent();
            CargarImagen();
            CargarDatos();
            ActualizarRolLabel();
        }

        private void CargarImagen()
        {
            var imagen = Imagen.ObtenerDesdeBase64(UsuarioService._usuarioActual.Image);
            imgPerfil.Source = imagen;
            mainWindow.imgAvatar.Source = imagen;
        }
        public void CargarDatos()
        {
            NodoSimple<Rol> actual = RolService._role.Cabeza;
            cbxCambiarRol.Items.Clear();
            cbxCambiarRol.Items.Add("[🛡️ Admin ]");
            while (actual != null)
            {
                cbxCambiarRol.Items.Add(actual.Dato.Nombre);

                actual = actual.Siguiente;
            }
        }

        private void btnRoles_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.frBody.Navigate(new RolesGui());
            mainWindow.frBody.Visibility = Visibility.Visible;            
        }

        private void ActualizarRolLabel()
        {
            var rolActual = UsuarioService._usuarioActual.RolActual;

            if (rolActual == null)
            {
                tbxRolActual.Text = "[🛡️ Admin ]";
                return;
            }

            tbxRolActual.Text = rolActual.Nombre;
        }

        private async void btnImageCambiar_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.Title = "Seleccionar imagen";
            openFile.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFile.ShowDialog() == true)
            {
                string ruta = openFile.FileName;
                await UsuarioService.GuardarImagenPerfilUsuario(ruta);
                CargarImagen();
            }
        }

        private void PreviewMouseDown_Click(object sender, MouseButtonEventArgs e)
        {
            CargarDatos();
        }

        private void cbxCambiarRol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            string? rol = cbxCambiarRol.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(rol)) return;
            
            if (rol == "[🛡️ Admin ]")
            {
                if(UsuarioService._usuarioActual.RolActual == null)
                {
                    MessageBox.Show("No puedes elegir el rol que tienes actualmente");
                    return;
                }

                mainWindow.frBody.Navigate(new Password(rol));
                mainWindow.frBody.Visibility = Visibility.Visible;
                if (!RolService._cambiarRol) return;

                UsuarioService._usuarioActual.RolActual = null;
                ActualizarRolLabel();
                mainWindow.MostrarBotonesSegunPermisos();
                return;
            }

            mainWindow.frBody.Navigate(new Password(rol));
            mainWindow.frBody.Visibility = Visibility.Visible;

            if (!RolService._cambiarRol) return;

            string? HayError = RolService.CambiarUsuarioRol(rol);
            if (HayError != null)
            {
                MessageBox.Show(HayError + "Rol a intentar agregar:" + rol);
                return;
            }

            ActualizarRolLabel();

            mainWindow.MostrarBotonesSegunPermisos();
            RolService._cambiarRol = false;
        }
    }
}
