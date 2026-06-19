using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using App.BackEnd.Services;

namespace App.Components.windows
{
    /// <summary>
    /// Lógica de interacción para Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        private bool isLoginMode;
        public Auth()
        {
            InitializeComponent();
            isLoginMode = true;
            LoginMode();
            lblCargando.Visibility = Visibility.Hidden;
        }
        private void lblAuth_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            if (!isLoginMode)
            {
                LoginMode();
                return;
            }

            brdPasswordRepeat.Visibility = Visibility.Visible;
            lblPasswordRepeat.Visibility = Visibility.Visible;
            lblAuthMode.Content = "¿Ya tienes una cuenta?"; btnAuth.Content = "Crear Cuenta";
            lblTitle.Content = "¡Te damos la bienvenida!";
            lblDescription.Content = "Gracias por unirte a nosotros";
            btnAuth.Click -= Login_Click;
            btnAuth.Click += Register_Click;
            this.KeyDown -= Login_Click;
            this.KeyDown += (s, args) =>
            {
                if (args.Key == Key.Enter)
                {
                    Register_Click(s, e);
                }
            };
            isLoginMode = false;
        }
        


        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            lblCargando.Visibility = Visibility.Visible;
            string? result = await AuthService.Login(txtUserName.Text, txtPasssword.Password);
            lblCargando.Visibility = Visibility.Hidden;
            if (result != null)
            {
                MessageBox.Show(result);
                return;
            }
            
            ViewDashboard();
        }


        private async void Register_Click(object sender, RoutedEventArgs e)
        {

            lblCargando.Visibility = Visibility.Visible;
            string? result = await AuthService.Register(txtUserName.Text, txtPasssword.Password, txtPasswordRepeat.Password);
            lblCargando.Visibility = Visibility.Hidden;
            if (result != null)
            {
                MessageBox.Show(result);
                return;
            }
            ViewDashboard();
        }

        private void ViewDashboard()
        {
            Dashboard dashboard = new Dashboard();
            Application.Current.MainWindow = dashboard;
            this.Hide();
            dashboard.Show();
            this.Close();
        }
        private void LoginMode()
        {
            brdPasswordRepeat.Visibility = Visibility.Hidden;
            lblPasswordRepeat.Visibility = Visibility.Hidden;
            lblAuthMode.Content = "¿Crear una cuenta?";
            btnAuth.Content = "Iniciar Sesión";
            lblTitle.Content = "¡Hola de nuevo!";
            lblDescription.Content = "Nos alegramos de volverte a ver";
            btnAuth.Click -= Register_Click;
            btnAuth.Click += Login_Click;
            this.KeyDown -= Register_Click;
            this.KeyDown += (s, args) =>
            {
                if (args.Key == Key.Enter)
                {
                    Login_Click(s, null);
                }
            };
            isLoginMode = true;
        }


        //---------------------------------------------------------------
        // Métodos para el funcionamiento dde UI :DD
        //---------------------------------------------------------------

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Text_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                if (tb.Text == (string)tb.Tag)
                {
                    tb.Text = "";
                    tb.Foreground = System.Windows.Media.Brushes.Black;
                }
                return;
            }

            if (sender is PasswordBox ps) //mijines si  es PasswordBox lo convierte a cast seguro y guarda en variable
            {
                if (ps.Password == (string)ps.Tag)
                {
                    ps.Password = "";
                    ps.Foreground = System.Windows.Media.Brushes.Black;
                }
            }
        }

        private void Text_LostFocus(object sender, RoutedEventArgs e)  
        {
            if (sender is TextBox tb)
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = (string)tb.Tag;
                    tb.Foreground = System.Windows.Media.Brushes.Gray;
                }

            if (sender is PasswordBox ps)
                if (string.IsNullOrWhiteSpace(ps.Password))
                {
                    ps.Password = (string)ps.Tag;
                    ps.Foreground = System.Windows.Media.Brushes.Gray;
                }
        }

        
    }
}
