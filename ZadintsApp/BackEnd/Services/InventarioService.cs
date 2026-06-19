using App.BackEnd.Domain.Entities;
using App.BackEnd.Domain.Enum;
using App.BackEnd.Domain.Nodo;
using App.BackEnd.Helpers;
using App.BackEnd.Services.ESDAD;
using System.Data;
using System.Windows.Controls;

namespace App.BackEnd.Services
{
    public class InventarioService
    {
        internal static ListaSimple<ProductoEntity> _ProductosInventario { get; set; } = new ListaSimple<ProductoEntity>();
        

        #region Métodos para manejar inventario
        public static async Task CargarProductos(string userEmail)
        {
            _ProductosInventario.EliminarTodo();

            DataTable dt = await DatabaseService.BuscarProductosPorCorreo(userEmail);

            if (dt.Rows.Count == 0) return;

            foreach (DataRow fila in dt.Rows)
            {
                CategoriasProductosEnum cat =Enum.Parse<CategoriasProductosEnum>(fila["Descripcion"].ToString());
                ProductoEntity newPlato = new ProductoEntity
                (
                    id: fila["Id"].ToString(),
                    nombre: fila["NombrePlato"].ToString(),
                    cartegoria: cat,
                    precio: Convert.ToDecimal(fila["Precio"]),
                    stock: Convert.ToInt32(fila["Stock"]),
                    foto64: Imagen.ObtenerDesdeBase64(fila["FotoRuta"].ToString())

                );
                _ProductosInventario.InsertarCola(newPlato);
            }
        }


        public static void AgregarPlato(string nombre, CategoriasProductosEnum cat, decimal precio, int stock, string imagen64)
        {

            string id = Guid.NewGuid().ToString();
            ProductoEntity producto = new ProductoEntity(id, nombre, cat, precio, stock, Imagen.ObtenerDesdeBase64(imagen64));
            _ProductosInventario.InsertarCola(producto);
            _ = DatabaseService.InsertarProductoAsync(UsuarioService._usuarioActual.Correo, producto, imagen64);
           
        }

        public static NodoSimple<ProductoEntity> ObtenerPlato(int id)
        {
            if(_ProductosInventario.Cabeza == null)
                return null;

            if (id <= 0)
                return  _ProductosInventario.Cabeza;

            NodoSimple<ProductoEntity> actual = _ProductosInventario.Cabeza;
            int cotador = 0;

            while (actual != null)
            {
                if (cotador == id)
                    return actual;

                actual = actual.Siguiente;
                cotador++;
            }

            return null;

        }

        public static void EliminarPlato(string id)
        {

            Predicate<ProductoEntity> predicate = new Predicate<ProductoEntity>(p => p.Id == id);
            _ProductosInventario.Eliminar(predicate);
            DatabaseService.EliminarProducto(id);
        }

        public static void CargarLista(ListBox lista)
        {
            lista.Items.Clear();
            int count = 0;
            NodoSimple<ProductoEntity> actual = _ProductosInventario.Cabeza;
            while (actual != null)
            {
                count++;
                lista.Items.Add(actual.Dato);
                actual = actual.Siguiente;
            }
        }

        public static int ContarPlatos()
        {
            int count = 0;
            NodoSimple<ProductoEntity> actual = _ProductosInventario.Cabeza;
            while (actual != null)
            {
                count++;
                actual = actual.Siguiente;
            }
            return count;
        }
        #endregion
    }
}