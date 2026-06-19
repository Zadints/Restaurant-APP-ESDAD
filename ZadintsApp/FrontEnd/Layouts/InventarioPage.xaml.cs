using App.BackEnd.Domain.Entities;
using App.BackEnd.Domain.Enum;
using App.BackEnd.Helpers;
using App.BackEnd.Services;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Zrutas.UI.Views.Content
{
    /// <summary>
    /// Lógica de interacción para Inventory.xaml
    /// </summary>
    public partial class Inventory : Page
    {
        private string _rutaFotoActual = "";
        
        public Inventory()
        {
            InitializeComponent();
           
            CargarBotones();
            //Loaded += async (_, __) => await InventarioService.CargarProductos(UsuarioService._usuarioActual.Correo);
            RefrescarLista();
        }

        #region Métodos para manejar el RolServices
        private void CargarBotones()
        {
            var RolActual = UsuarioService._usuarioActual.RolActual;
            if (RolActual == null) return;

            if (!RolActual.Permisos.EliminarProductos)
            {
                btnEliminarProductos.IsEnabled = false;
            }

            if (!RolActual.Permisos.AgregarProductos)
            {
                btnAgregarProductos.IsEnabled = false;
            }            
        }
        #endregion
        #region Métodos para manejar UI Inventario
        private void RefrescarLista()
        {
            InventarioService.CargarLista(LstPlatos);
            TxtTotal.Text = "Total de productos: " + InventarioService.ContarPlatos();
        }

        private void BtnSeleccionarFoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialogo = new OpenFileDialog();
            dialogo.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp";
            dialogo.ShowDialog();

            if (!string.IsNullOrWhiteSpace(dialogo.FileName))
            {
                
                _rutaFotoActual = dialogo.FileName;
                ImgFotoPlato.Source = new BitmapImage(new Uri(dialogo.FileName));
            }
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {

            string nombre = TxtNombre.Text.Trim();
            string precioStr = TxtPrecio.Text.Trim();
            string stockStr = TxtStock.Text.Trim();

            if (nombre == "Ej: Lomo Saltado") nombre = "";
            if (precioStr == "Ej: 25.00") precioStr = "";
            if (stockStr == "Ej: 10") stockStr = "";

            if (cbxCategoria.SelectedItem == null)
            {
                MessageBox.Show("Seleccione una categoría.");
                return;
            }
            CategoriasProductosEnum cat =Enum.Parse<CategoriasProductosEnum>(((ComboBoxItem)cbxCategoria.SelectedItem).Content.ToString());

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("El nombre del plato es obligatorio.", "Aviso");
                return;
            }

            if (!decimal.TryParse(precioStr, out decimal precio) || precio <= 0)
            {
                MessageBox.Show("Ingresa un precio válido.", "Aviso");
                return;
            }

            if (!int.TryParse(stockStr, out int stock) || stock < 0)
            {
                MessageBox.Show("Ingresa un stock válido.", "Aviso");
                return;
            }


            InventarioService.AgregarPlato(nombre, cat, precio, stock, Imagen.ConvertirABase64(_rutaFotoActual));
            RefrescarLista();

            TxtNombre.Text = "Ej: Lomo Saltado";
            TxtNombre.Foreground = System.Windows.Media.Brushes.Gray;
            cbxCategoria.SelectedIndex = -1;
            TxtPrecio.Text = "Ej: 25.00";
            TxtPrecio.Foreground = System.Windows.Media.Brushes.Gray;
            TxtStock.Text = "Ej: 10";
            TxtStock.Foreground = System.Windows.Media.Brushes.Gray;
            ImgFotoPlato.Source = null;
            _rutaFotoActual = "";
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (LstPlatos.SelectedItem == null || !(LstPlatos.SelectedItem is ProductoEntity))
            {
                MessageBox.Show("Selecciona un plato de la lista.", "Aviso");
                return;
            }

            ProductoEntity seleccionado = (ProductoEntity)LstPlatos.SelectedItem;

            MessageBoxResult resultado = MessageBox.Show(
                "¿Eliminar \"" + seleccionado.Nombre + "\" del inventario?",
                "Confirmar",
                MessageBoxButton.YesNo);

            if (resultado == MessageBoxResult.Yes)
            {
                InventarioService.EliminarPlato(seleccionado.Id);
                RefrescarLista();
                ImgFotoPlato.Source = null;
                _rutaFotoActual = "";
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == (string)tb.Tag)
            {
                tb.Text = "";
                tb.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = (string)tb.Tag;
                tb.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }
        #endregion
    }
}
