using App.BackEnd.Domain.Entities;
using App.BackEnd.Services.ESDAD;
using System.Timers;

namespace App.BackEnd.Services
{
    internal class MenuPrincipalService
    {
        public static MenuPrincipalEntity _datosVentasIngresos { get; set; } = new MenuPrincipalEntity();
        private static string _correoUser;

        static MenuPrincipalService()
        {
            string tempCorreo = UsuarioService._usuarioActual.Correo;
            if (string.IsNullOrWhiteSpace(tempCorreo))
            {
                throw new ArgumentException("En MenuPrincipalService el correo es null o vacio");
            }
            _correoUser = tempCorreo;
        }

        private static async Task CargarVentas()
        {
            MenuPrincipalEntity? value = await DatabaseService.ObtenerStats(_correoUser);
            if (value is null) return;

            _datosVentasIngresos.ProductosVendidosAyer = value.ProductosVendidosAyer;
            _datosVentasIngresos.IngresoRecaudado = value.IngresoRecaudado;
        }

        internal static async Task ActualizarVentas(int ventas, decimal ingresoTotalHoy)
        {
            _datosVentasIngresos.IngresoTotalHoy += ingresoTotalHoy;
            _datosVentasIngresos.ProductosVendidosHoy += ventas;
            _ = DatabaseService.SumarVentasAyer(_correoUser, ventas, ingresoTotalHoy);
        }

        internal static async Task CargarVentasMaquinas()
        {
            await Task.WhenAll(
                CargarVentas(),
                MaquinasServices.CargarMaquinasConectadas()
            );
        }
    }
}
