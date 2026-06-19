using System;
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

namespace App.Components.windows
{
    /// <summary>
    /// Lógica de interacción para TicketVenta.xaml
    /// </summary>
    public partial class TicketVenta : Window
    {
        public TicketVenta()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public void CargarDatos(int numeroTicket, string detalle, decimal total)
        {
            DateTime ahora = DateTime.Now;

            txtTicket.Text = "Ticket N°: " + numeroTicket;

            txtFecha.Text = "Fecha: " + ahora.ToString("dd/MM/yyyy");

            txtHora.Text = "Hora: " + ahora.ToString("HH:mm:ss");

            string[] lineas = detalle.Split('\n');

            foreach (string linea in lineas)
            {
                if (!string.IsNullOrWhiteSpace(linea))
                {
                    lstDetalle.Items.Add(linea);
                }
            }
            txtTotal.Text =
                "TOTAL: S/ " + total.ToString("0.00");
        }
    }
}