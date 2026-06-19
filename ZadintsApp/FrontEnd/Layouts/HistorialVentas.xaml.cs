using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Zrutas.UI.Views.Content;
using App.Domain.Entities;
using App.Services.Ventas;
using App.BackEnd.Domain.Nodo;
using App.BackEnd.Services.ESDAD;

namespace App.Components.Layouts.Content
{
    /// <summary>
    /// Lógica de interacción para HistorialVentas.xaml
    /// </summary>
    public partial class HistorialVentas : Page
    {
        public HistorialVentas()
        {
            InitializeComponent();
            CargarVentas();
        }
        private async void CargarVentas()
        {
            lstVentas.Items.Clear();

            ListaSimple<TicketResumen>? lista = await VentasService.ObtenerTodasLasVentas();
            if (lista == null) return;

            NodoSimple<TicketResumen> actual = lista.Cabeza;

            while (actual != null)
            {
                lstVentas.Items.Add(actual.Dato);

                actual = actual.Siguiente;
            }
        }
        private void BtnMostrarTodo_Click(object sender, RoutedEventArgs e)
        {
            CargarVentas();
        }
        private async void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (dpInicio.SelectedDate == null || dpFin.SelectedDate == null)
            {
                MessageBox.Show("Seleccione ambas fechas.");
                return;
            }
            lstVentas.Items.Clear();

            ListaSimple<TicketResumen>? lista = await VentasService.ObtenerVentasPorFecha(dpInicio.SelectedDate.Value, dpFin.SelectedDate.Value);
            if (lista == null) return;
            NodoSimple<TicketResumen> actual = lista.Cabeza;

            while (actual != null)
            {
                lstVentas.Items.Add(actual.Dato);
                actual = actual.Siguiente;
            }
        }
        private async void lstVentas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstVentas.SelectedItem == null)
                return;

            TicketResumen ticket = (TicketResumen)lstVentas.SelectedItem;

            lblTicket.Text = "Ticket #" + ticket.NumeroTicket;

            lblFecha.Text = ticket.Fecha;

            lblTotal.Text = "Total: S/" + ticket.Total;

            lstDetalle.Items.Clear();

            DataTable? dt = await VentasService.ObtenerDetalleTicket(ticket.IdVenta);

            if (dt == null)
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
    }
}
