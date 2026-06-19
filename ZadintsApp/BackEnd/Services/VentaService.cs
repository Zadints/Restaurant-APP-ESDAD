using App.BackEnd.Domain.Entities;
using App.BackEnd.Domain.Nodo;
using App.BackEnd.Services;
using App.BackEnd.Services.ESDAD;
using App.Components.Layouts.Content;
using App.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using Zrutas.UI.Views.Content;


namespace App.Services.Ventas
{
    public class VentasService
    {
        internal static Colas<TicketResumen> _historialVentas { get; set; } = new Colas<TicketResumen>();
        internal static ArbolesBinarios<VentasEntity> _productosTop { get; set; } = new ArbolesBinarios<VentasEntity>();
        internal static int _numeroTicket { get; set; }


        internal static async Task CargarTicketActual()
        {
            _numeroTicket = await ObtenerSiguienteTicket();
        }

        internal static void InsertarProductosTop(VentasEntity producto)
        {
            _productosTop.Insertar(producto, (a, b) => a.Plato.Nombre.CompareTo(b.Plato.Nombre), 
                (existente, nuevo) =>
            {
                existente.Cantidad += nuevo.Cantidad;
            });
        }


        public static async Task RegistrarVenta( decimal total, ListaSimple<VentasEntity> carrito)
        {
            _numeroTicket++;
            DateTime fecha = DateTime.Now;
            string idVenta = Guid.NewGuid().ToString();
           
            _ = DatabaseService.GuardarVentaMySQL(idVenta, UsuarioService._usuarioActual.Correo, _numeroTicket, fecha, total);
            var actual = carrito.Cabeza;

            while (actual != null)
            {
                string idDetalle = Guid.NewGuid().ToString();
                decimal subtotal = actual.Dato.Plato.Precio * actual.Dato.Cantidad;

                _ = DatabaseService.GuardarDetalleVentaMySQL(idDetalle, idVenta, actual.Dato.Plato.Id, actual.Dato.Plato.Nombre, actual.Dato.Cantidad, actual.Dato.Plato.Precio, subtotal);


                InsertarProductosTop(actual.Dato);
                var newVEntas = new TicketResumen();
                newVEntas.IdVenta = idVenta;
                newVEntas.NumeroTicket = _numeroTicket;
                newVEntas.Fecha = fecha.ToString();
                newVEntas.Total = total;
                newVEntas.Foto64 = actual.Dato.Plato.Foto64;

                _historialVentas.Queue(newVEntas);
                actual = actual.Siguiente;
            }

           
        }
       

        public static async Task<int> ObtenerSiguienteTicket()
        {
            return await DatabaseService.ObtenerUltimoNumeroTicket();
        }

        public static async Task<ListaSimple<TicketResumen>?> ObtenerTodasLasVentas()
        {
            return await DatabaseService.ObtenerTodasLasVentas();
        }

        public static async Task<ListaSimple<TicketResumen>?> ObtenerVentasPorFecha(DateTime inicio, DateTime fin)
        {
            return await DatabaseService.ObtenerVentasPorFecha(inicio, fin);
        }

        public static async Task<DataTable?> ObtenerDetalleTicket(string idVenta)
        {
            return await DatabaseService.ObtenerDetalleTicket(idVenta);
        }
        public static async Task RestarStockProducto(int cantidad , string productoId)
        {
            _ = await DatabaseService.RestarStockProducto(cantidad, productoId); ;
        }



        public static async Task<decimal> ObtenerIngresosHoy()
        {
            return await DatabaseService.ObtenerIngresosHoy();
        }
        public static async Task<decimal> ObtenerIngresoTotal()
        {
            return await DatabaseService.ObtenerIngresoTotal();
        }
        public static async Task<decimal> ObtenerIngresoPorFecha(DateTime inicio, DateTime fin)
        {
            return await DatabaseService.ObtenerIngresoPorFecha(inicio, fin);
        }
        public static async Task<ListaSimple<ProductosIngreso>?> ObtenerProductosMasRentables()
        {
            return await DatabaseService.ObtenerProductosMasRentables();
        }
        public static async Task<ListaSimple<ProductosIngreso>?> ObtenerProductosPorFecha(DateTime inicio, DateTime fin)
        {
            return await DatabaseService.ObtenerProductosPorFecha(inicio, fin) ;
        }


    }
}
