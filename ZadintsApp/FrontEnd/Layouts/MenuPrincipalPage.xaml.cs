using App.BackEnd.Domain.Entities;
using App.BackEnd.Domain.Nodo;
using App.BackEnd.Services;
using App.BackEnd.Services.ESDAD;
using App.Components.Layouts.Body;
using App.Components.windows;
using App.Services.Ventas;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Zrutas.UI.Views.Content
{
    /// <summary>
    /// Lógica de interacción para MenuPrincipalPage.xaml
    /// </summary>
    public partial class MenuPrincipalPage : Page
    {
        private readonly DispatcherTimer _timer;
        public MenuPrincipalPage()
        {
            InitializeComponent();
            CargarTop();
            Loaded += async (_, __) => await ActualizarTop();
            

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMinutes(1);
            _timer.Tick += async (_, __) => await ActualizarTop();
            _timer.Start();
        }

        private async Task ActualizarTop()
        {
            
            lstMaquinasConectadas.Items.Clear();
            lstMaquinasConectadas.Items.Add("Cargando...");
            await MenuPrincipalService.CargarVentasMaquinas();
            

            lblVentasHoy.Text = MenuPrincipalService._datosVentasIngresos.ProductosVendidosHoy.ToString();
            lblIngresoTotalHoy.Text = "S/." + MenuPrincipalService._datosVentasIngresos.IngresoTotalHoy.ToString();
            lblIngresoTotalRecaudado.Text = "S/." + MenuPrincipalService._datosVentasIngresos.IngresoRecaudado.ToString();
            lblVentasAyer.Text = MenuPrincipalService._datosVentasIngresos.ProductosVendidosAyer.ToString();

            lstMaquinasConectadas.Items.Clear();
            NodoSimple<MaquinaConectadaEntity> actual = MaquinasServices._maquinasConectadas.Cabeza;
            while (actual != null)
            {
                lstMaquinasConectadas.Items.Add(actual.Dato);

                actual = actual.Siguiente;
            }
        }

        private void CargarTop()
        {
            ListaSimple<VentasEntity> lista = VentasService._productosTop.InOrden();

            NodoSimple<VentasEntity> actual = lista.Cabeza;
            if(actual == null) return;

            lblProducto1.Text = actual.Dato.Plato.Nombre;
            lblVentas1.Text = actual.Dato.Cantidad.ToString();
            actual = actual.Siguiente;

            if (actual == null) return;

            lblProducto2.Text = actual.Dato.Plato.Nombre;
            lblVentas2.Text = actual.Dato.Cantidad.ToString();
            actual = actual.Siguiente;

            if (actual == null) return;

            lblProducto3.Text = actual.Dato.Plato.Nombre;
            lblVentas3.Text = actual.Dato.Cantidad.ToString();
            actual = actual.Siguiente;

            if (actual == null) return;
            lblProducto4.Text = actual.Dato.Plato.Nombre;
            lblVentas4.Text = actual.Dato.Cantidad.ToString();
        }

        private void Navigate()
        {
            Dashboard mainWindow = (Dashboard)Application.Current.MainWindow;
            mainWindow.frContent.Navigate(new VentasPage());
            mainWindow.frContent.Visibility = Visibility.Visible;
        }

        private void btnTop1_Click(object sender, RoutedEventArgs e)
        {
            Navigate();
        }   

        private void btnTop2_Click(object sender, RoutedEventArgs e)
        {
            Navigate();
        }

        private void btnTop3_Click(object sender, RoutedEventArgs e)
        {
            Navigate();
        }

        private void btnTop4_Click(object sender, RoutedEventArgs e)
        {
            Navigate();
        }
    }
}
