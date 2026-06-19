using App.BackEnd.Domain.Entities;
using App.BackEnd.Domain.Nodo;
using App.BackEnd.Services;
using App.BackEnd.Services.ESDAD;
using App.Components.Layouts.Content;
using App.Components.windows;
using App.Services.Ventas;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Zrutas.UI.Views.Content
{
    /// <summary>
    /// Lógica de interacción para Selling.xaml
    /// </summary>
    public partial class VentasPage : Page
    {
        private ListaSimple<VentasEntity> carrito { get; set; } = new ListaSimple<VentasEntity>();
        public static ListaSimple<VentasEntity> _listaPlatosVendidos = new ListaSimple<VentasEntity>();
        public VentasPage()
        {
            InitializeComponent();
            CargarProductos();
            
        }
        private void CargarProductos()
        {

            NodoSimple<ProductoEntity> actual = InventarioService._ProductosInventario.Cabeza;

            while (actual != null)
            {
                AgregarCardProducto(actual.Dato);
                actual = actual.Siguiente;
            }
        }
        private void cbxCategoriasuBracsuB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxCategoriasuBracsuB.SelectedItem is ComboBoxItem item)
            {
                string categoria = item.Content.ToString();

                NodoSimple<ProductoEntity> actual = InventarioService._ProductosInventario.Cabeza;
                wpProductos.Children.Clear();
                while (actual != null)
                {
                    if (actual.Dato.Cartegoria.ToString().Equals(categoria))
                    {
                        AgregarCardProducto(actual.Dato);
                    }
                    actual = actual.Siguiente;
                }
            }
        }

        

        private void AgregarCardProducto(ProductoEntity plato)
        {
            Border contenedor = new Border();

            contenedor.Width = 140;
            contenedor.Height = 190;
            contenedor.Margin = new Thickness(10);
            contenedor.CornerRadius = new CornerRadius(10);
            contenedor.BorderBrush = Brushes.LightGray;
            contenedor.BorderThickness = new Thickness(1);
            contenedor.Background = Brushes.White;

            Button card = new Button();

            card.BorderThickness = new Thickness(0);
            card.Background = Brushes.Transparent;
            card.Cursor = Cursors.Hand;

            StackPanel panel = new StackPanel();

            Image imagen = new Image();

            imagen.Source = plato.Foto64;
            imagen.Height = 75;
            imagen.Width = 75;
            imagen.Stretch = Stretch.UniformToFill;

            TextBlock categoria = new TextBlock();

            categoria.Text = plato.Cartegoria.ToString();
            categoria.FontSize = 13;
            categoria.FontWeight = FontWeights.Light;
            categoria.TextAlignment = TextAlignment.Center;
            categoria.Foreground = Brushes.Orange;
            categoria.Margin = new Thickness(4);

            TextBlock nombre = new TextBlock();

            nombre.Text = plato.Nombre;
            nombre.FontSize = 15;
            nombre.FontWeight = FontWeights.Bold;
            nombre.TextAlignment = TextAlignment.Center;
            nombre.Margin = new Thickness(5);

            Grid etiquetas = new Grid();

            etiquetas.Margin = new Thickness(5);

            etiquetas.ColumnDefinitions.Add(new ColumnDefinition());

            etiquetas.ColumnDefinitions.Add(new ColumnDefinition());

            Border precioBox = new Border();

            precioBox.Background = Brushes.ForestGreen;
            precioBox.CornerRadius = new CornerRadius(5);
            precioBox.Padding = new Thickness(5);

            TextBlock precio = new TextBlock();

            precio.Text = "S/ " + plato.Precio;
            precio.Foreground = Brushes.White;
            precio.FontWeight = FontWeights.Bold;

            precioBox.Child = precio;

            Border stockBox = new Border();

            stockBox.Background = Brushes.ForestGreen;
            stockBox.CornerRadius = new CornerRadius(5);
            stockBox.Padding = new Thickness(5);

            TextBlock stock = new TextBlock();

            stock.Text = "Stock " + plato.Stock;
            stock.Foreground = Brushes.White;
            stock.FontWeight = FontWeights.Bold;

            stockBox.Child = stock;

            Grid.SetColumn(precioBox, 0);
            Grid.SetColumn(stockBox, 1);

            etiquetas.Children.Add(precioBox);
            etiquetas.Children.Add(stockBox);

            panel.Children.Add(imagen);
            panel.Children.Add(nombre);
            panel.Children.Add(categoria);
            panel.Children.Add(etiquetas);

            card.Content = panel;

            if (plato.Stock <= 0)
            {
                card.IsEnabled = false;
                contenedor.Opacity = 0.5;

                TextBlock agotado = new TextBlock();

                agotado.Text = "AGOTADO";
                agotado.Foreground = Brushes.Red;
                agotado.FontWeight = FontWeights.Bold;
                agotado.HorizontalAlignment = HorizontalAlignment.Center;

                panel.Children.Add(agotado);
            }

            card.Click += (s, e) =>
            {
                AgregarAlCarrito(plato);
            };

            contenedor.Child = card;

            wpProductos.Children.Add(contenedor);
        
        }
        private void AgregarAlCarrito(ProductoEntity plato)
        {
            if (plato.Stock <= 0)
                return;

            plato.Stock--;

            NodoSimple<VentasEntity> actual = carrito.Cabeza;

            while (actual != null)
            {
                if (actual.Dato.Plato.Id == plato.Id)
                {
                    actual.Dato.Cantidad++;

                    ActualizarLista();

                    return;
                }
                actual = actual.Siguiente;
            }
            carrito.InsertarCabeza(new VentasEntity(plato));
            ActualizarLista();
            
        }
        private void RecargarCards()
        {
            wpProductos.Children.Clear();
            CargarProductos();
        }
        private void ActualizarLista()
        {
            decimal total = 0;
            int cantidadTotal = 0;
            NodoSimple<VentasEntity> actual = carrito.Cabeza;
            lstProductosAVender.Items.Clear();
            while (actual != null)
            {
                decimal subtotal = actual.Dato.Plato.Precio * actual.Dato.Cantidad;

                lstProductosAVender.Items.Add(actual.Dato.Plato.Nombre + "             |             " + actual.Dato.Cantidad + "              |              S/ " + subtotal.ToString("0.00"));
                total += subtotal;
                cantidadTotal += actual.Dato.Cantidad;
                actual = actual.Siguiente;
            }
            lblTotalPrecio.Text = "S/ " + total.ToString("0.00");
            lblTotalProductos.Text = cantidadTotal.ToString();
        }
        private async void btnVender_Click(object sender, RoutedEventArgs e)
        {
            
            if (carrito.Cabeza == null)
            {
                MessageBox.Show("No hay productos seleccionados.");
                return;
            }

            string detalle = "";
            decimal total = 0;
            int cantidadVentas = 0;
            
            
            NodoSimple<VentasEntity> actual = carrito.Cabeza;

            while (actual != null)
            {
                decimal subtotal = actual.Dato.Plato.Precio * actual.Dato.Cantidad;
                cantidadVentas += actual.Dato.Cantidad;
                detalle += actual.Dato.Plato.Nombre + " x" + actual.Dato.Cantidad + "    S/ " + subtotal.ToString("0.00") + "\n";
                total += subtotal;
                _listaPlatosVendidos.InsertarCabeza(actual.Dato);
                await VentasService.RestarStockProducto(actual.Dato.Cantidad, actual.Dato.Plato.Id);                
                actual = actual.Siguiente;
                
            }

            TicketVenta ticket = new TicketVenta();
            ticket.CargarDatos(VentasService._numeroTicket, detalle, total);
            ticket.ShowDialog();

            await VentasService.RegistrarVenta(total, carrito);
            await MenuPrincipalService.ActualizarVentas(cantidadVentas, total);

            carrito.EliminarTodo();
            ActualizarLista();
            RecargarCards();
        }

        private void btnEliminarTodo_Click(object sender, RoutedEventArgs e)
        {
            NodoSimple<VentasEntity> actual = carrito.Cabeza;

            while (actual != null)
            {
                actual.Dato.Plato.Stock += actual.Dato.Cantidad;
                actual = actual.Siguiente;
            }
            carrito.EliminarTodo();
            ActualizarLista();
            RecargarCards();
        }

        private void btnEliminarSeleccionado_Click(object sender, RoutedEventArgs e)
        {
            int indice = lstProductosAVender.SelectedIndex;

            if (indice < 0)
                return;

            NodoSimple<VentasEntity> actual = carrito.Cabeza;

            int contador = 0;

            while (actual != null)
            {
                if (contador == indice)
                {
                    actual.Dato.Plato.Stock++;

                    actual.Dato.Cantidad--;

                    if (actual.Dato.Cantidad <= 0)
                    {
                        carrito.Eliminar(x => x.Plato.Id == actual.Dato.Plato.Id);
                    }
                    break;
                }
                contador++;
                actual = actual.Siguiente;
            }
            ActualizarLista();
            RecargarCards();
        }

        private void btnLimpiarFiltro_Click(object sender, RoutedEventArgs e)
        {
            RecargarCards();
            cbxCategoriasuBracsuB.SelectedIndex = -1;
        }
    }
}
