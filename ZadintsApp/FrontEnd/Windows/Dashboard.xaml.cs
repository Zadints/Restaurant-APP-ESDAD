using App.BackEnd.Domain.Entities;
using App.BackEnd.Helpers;
using App.BackEnd.Services;
using App.Components.Layouts.Content;
using System.Windows;
using System.Windows.Controls;
using Zrutas.UI.Views.Content;
using Zrutas.UI.Views.Frames;

namespace App.Components.windows
{
    public partial class Dashboard : Window
    {
        public Dashboard()
        {
            InitializeComponent();
            frBody.Visibility = Visibility.Collapsed;
            frContent.Navigate(new MenuPrincipalPage());
            imgAvatar.Source = Imagen.ObtenerDesdeBase64(UsuarioService._usuarioActual.Image);
        }
        /*-------------------------------------------
         * Sidebar Buttons Content
         ------------------------------------------------*/
        private void btnMain_Click(object sender, RoutedEventArgs e)
        {
            frContent.Navigate(new MenuPrincipalPage());
        }
        private void btnInventory_Click(object sender, RoutedEventArgs e)
        {
            frContent.Navigate(new Inventory());

        }
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            frContent.Navigate(new Setting());
        }

        private void btnVender_Click(object sender, RoutedEventArgs e)
        {
            frContent.Navigate(new VentasPage());
        }

        private void btnNews_Click(object sender, RoutedEventArgs e)
        {
            //No contamos con APi por el momento
        }

        private void btnHistorialVentas_Click(object sender, RoutedEventArgs e)
        {
            frContent.Navigate(new HistorialVentas());
        }

        private void btnPedidos1_Click(object sender, RoutedEventArgs e)
        {
            frContent.Navigate(new PedidosActuales());
        }

        /*----------------------------------
         Eventos para los roles
        ----------------------------------------*/

        public void MostrarBotonesSegunPermisos()
        {
            var Roles = UsuarioService._usuarioActual.RolActual;

            if (Roles == null)
            {
                
                btnVender.IsEnabled = true;
                btnHistorialVentas.IsEnabled = true;
                return;
            }

            
            var permisos = Roles.Permisos;
            CargarBotones(permisos);
            
        }

        private void CargarBotones(PermisosEntity permisos)
        {
            IsEnabledButtons(btnVender, permisos.VenderProductos);
            IsEnabledButtons(btnHistorialVentas, permisos.VerClientes);

        }

        private void IsEnabledButtons(Button button, bool permiso)
        {
            if (!permiso)
            {
                button.IsEnabled = false;                
                return;
            }
            button.IsEnabled = true;
        }

        /*----------------------------------
         Eventos para los btones de cerrar sesión y app
        ----------------------------------------*/

        private void btnCloseSession_Click(object sender, RoutedEventArgs e)
        {
            Auth login = new Auth();
            Application.Current.MainWindow = login;

            login.Show();
            MaquinasServices.EstablecerDesconectado();
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MaquinasServices.EstablecerDesconectado();
            this.Close();
        }


        /*------------------------------------
         * Método del botón Anuncios , se encarga de mostrar las notificaciones a travez del tab 4 al usuario, 
         * se llama cada vez que el usuario hace click en el botón de novedades
         ------------------------------------*/
        private void Frame_Content(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }

        
    }
}
