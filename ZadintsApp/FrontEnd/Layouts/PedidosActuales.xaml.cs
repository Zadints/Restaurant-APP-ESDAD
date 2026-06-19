using App.BackEnd.Domain.Nodo;
using App.BackEnd.Services.ESDAD;
using App.Components.windows;
using App.Domain.Entities;
using App.Services.Ventas;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Zrutas.UI.Views.Content;
using System.Windows.Threading;



namespace App.Components.Layouts.Content
{
    public partial class PedidosActuales : Page
    {
        private DispatcherTimer _timer;

        public PedidosActuales()
        {
            InitializeComponent();

            CargarTickets();
            Seleccion();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(30); 
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CargarTickets();
        }
        private void CargarTickets()
        {
            NodoSimple<TicketResumen> ventas = VentasService._historialVentas.Frente;

            lstTickets.Items.Clear();
            while (ventas != null)
            {

                lstTickets.Items.Add(ventas.Dato);
                
                ventas = ventas.Siguiente;
            }
        }

        private async Task Seleccion()
        {
            if (lstTickets.Items.Count == 0)
            {
                btnAtendidos.IsEnabled = false;
                btnAgregarPedido.IsEnabled = false;
                lstTickets.Items.Clear();
                lstDetalle.Items.Clear();
                return;
            }
            btnAtendidos.IsEnabled = true;
            btnAgregarPedido.IsEnabled = false;

            lstTickets.SelectedIndex = 0;

            TicketResumen ticket = (TicketResumen)lstTickets.SelectedItem;

            lblTicket.Text = "Ticket #" + ticket.NumeroTicket;
            lblFecha.Text = ticket.Fecha;
            lblTotal.Text = "Total: S/" + ticket.Total;

            lstDetalle.Items.Clear();

            DataTable? dt = await VentasService.ObtenerDetalleTicket(ticket.IdVenta);

            if (dt == null || dt.Rows.Count == 0)
                return;

            foreach (DataRow fila in dt.Rows)
            {
                lstDetalle.Items.Add(
                    fila["NombreProducto"]
                    + " x"
                    + fila["Cantidad"]
                    + " = S/"
                    + fila["Subtotal"]);
            }
        }

        private async void lstTickets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                return;
            
        }
        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            Dashboard dashboard = (Dashboard)Application.Current.MainWindow;
            dashboard.frContent.Navigate(new MenuPrincipalPage());
        }

        private void btnAtendidos_Click(object sender, RoutedEventArgs e)
        {
            VentasService._historialVentas.Dequeue();
            CargarTickets();
            Seleccion();
        }

        private void btnAgregarPedido_Click(object sender, RoutedEventArgs e)
        {
            Dashboard mainWindow = (Dashboard)Application.Current.MainWindow;
            mainWindow.frContent.Navigate(new VentasPage());
            mainWindow.frContent.Visibility = Visibility.Visible;
        }
    }
}
