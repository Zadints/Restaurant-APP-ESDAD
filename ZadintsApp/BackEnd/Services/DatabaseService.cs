using App.BackEnd.Domain.Entities;
using App.BackEnd.Helpers;
using App.BackEnd.Services.ESDAD;
using App.Domain.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;

namespace App.BackEnd.Services
{
    internal class DatabaseService
    {
        private const string _coneccion =
    "Server=0.0.0.0;" +
    "Port=3306;" +
    "Database=none;" +
    "Uid=admin;" +
    "Pwd=password;" +
    "SslMode=None;" +
    "AllowPublicKeyRetrieval=True;" +
    "Connection Timeout=5;" +
    "Pooling=false;";

        #region AuthServices region
        internal static async Task<string?> BuscarPorCorreo(string correoUsuario)
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(_coneccion);

                await conn.OpenAsync();

                const string sql = @"
SELECT UserPassword
FROM users
WHERE UserMail = @correo
LIMIT 1";

                using MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@correo", correoUsuario);

                object? resultado = await cmd.ExecuteScalarAsync();

                if (resultado == null || resultado == DBNull.Value)
                    return null;

                return resultado.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        internal static bool ExisteCorreoMySQL(string correoUsuario)
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(_coneccion);

                conn.Open();

                const string sql = @"
            SELECT COUNT(*)
            FROM users
            WHERE UserMail = @correo";

                using MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@correo", correoUsuario);

                object? result = cmd.ExecuteScalar(); 

                int cantidad = Convert.ToInt32(result ?? 0);

                return cantidad > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        internal static bool InsertarUsuarioMySQL(
            string userName,
            string userMail,
            string userImage,
            string userPassword)
        {
            using MySqlConnection conn = new MySqlConnection(_coneccion);

            conn.Open();

            const string sql = @"
                INSERT INTO users
                (
                    UserName,
                    UserMail,
                    UserImage,
                    UserPassword
                )
                VALUES
                (
                    @UserName,
                    @UserMail,
                    @UserImage,
                    @UserPassword
                );";

            using MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@UserName", userName);
            cmd.Parameters.AddWithValue("@UserMail", userMail);
            cmd.Parameters.AddWithValue("@UserImage", userImage);
            cmd.Parameters.AddWithValue("@UserPassword", userPassword);

            int filasAfectadas = cmd.ExecuteNonQuery();

            return filasAfectadas > 0;
        }
        #endregion


        #region ImagenService

        internal static async Task ActualizarImgUsuarioMySQL(string userMail, string userImage)
        {
            using MySqlConnection conn = new MySqlConnection(_coneccion);

            await conn.OpenAsync();

            const string sql = @"
        UPDATE users
        SET UserImage = @UserImage
        WHERE UserMail = @UserMail;
    ";

            using MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserMail", userMail);
            cmd.Parameters.AddWithValue("@UserImage", userImage);

            await cmd.ExecuteNonQueryAsync();

        }

        internal static async Task<string?> ObtenerImgUsuarioMySQL(string userMail)
        {
            using MySqlConnection conn = new MySqlConnection(_coneccion);

            await conn.OpenAsync();

            const string sql = @"
        SELECT UserImage FROM users WHERE UserMail = @UserMail";

            using MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserMail", userMail);

            object? resultado = await cmd.ExecuteScalarAsync();

            if (resultado == null || resultado == DBNull.Value)
                return null;

            return resultado.ToString();
        }


        #endregion

        #region InventarioServices
        internal static async Task<DataTable> BuscarProductosPorCorreo(string correoUsuario)
        {
            await using MySqlConnection conn = new MySqlConnection(_coneccion);

            await conn.OpenAsync();

            const string sql = @"
        SELECT *
        FROM productos
        WHERE CorreoUsuario = @CorreoUsuario";

            await using MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@CorreoUsuario", correoUsuario);

            await using MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();

            DataTable dt = new DataTable();
            dt.Load(reader);

            return dt;
        }

        internal static async Task InsertarProductoAsync(string correoUsuario, ProductoEntity producto, string imagen64)
        {
            try
            {
                await using MySqlConnection conn = new MySqlConnection(_coneccion);

                await conn.OpenAsync();

                const string sql = @"
            INSERT INTO productos
            (CorreoUsuario, Id, NombrePlato, Descripcion, Precio, Stock, FotoRuta)
            VALUES
            (@CorreoUsuario, @Id, @NombrePlato, @Descripcion, @Precio, @Stock, @FotoRuta)";

                await using MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@CorreoUsuario", correoUsuario);
                cmd.Parameters.AddWithValue("@Id", producto.Id);
                cmd.Parameters.AddWithValue("@NombrePlato", producto.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", producto.Cartegoria.ToString());
                cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                cmd.Parameters.AddWithValue("@Stock", producto.Stock);
                cmd.Parameters.AddWithValue("@FotoRuta", imagen64);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        internal static void EliminarProducto(string id)
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(_coneccion);

                conn.Open();

                const string sql = @"
            DELETE FROM productos
            WHERE Id = @Id";

                using MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Menu principal Services 
        internal static async Task<MenuPrincipalEntity?> ObtenerStats(string correoUsuario)
        {
            using MySqlConnection conn = new MySqlConnection(_coneccion);

            await conn.OpenAsync();

            const string sql = @"
SELECT *
FROM stats
WHERE correoUsuario = @correoUsuario
LIMIT 1";

            using MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@correoUsuario", correoUsuario);

            using MySqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return new MenuPrincipalEntity
            {
                


                ProductosVendidosAyer = reader["ProductosVendidosAyer"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(reader["ProductosVendidosAyer"]),
                IngresoRecaudado = reader["IngresoRecaudado"] == DBNull.Value
                    ? null
                    : Convert.ToDecimal(reader["IngresoRecaudado"])
            };
        }


        #endregion


        #region Rol Srevices 
        internal static async Task<int> ContarRoles(string nombre, string userMail)
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(_coneccion);

                await conn.OpenAsync();

                const string sql = @"
            SELECT COUNT(*)
            FROM roles
            WHERE Nombre = @nombre
              AND Usermail = @userMail";

                using MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@userMail", userMail);

                object? resultado = await cmd.ExecuteScalarAsync();

                return (resultado == null || resultado == DBNull.Value)
                    ? 0
                    : Convert.ToInt32(resultado);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        internal static async Task InsertarRol(
    string nombre,
    string descripcion,
    string permisosId,
    string userMail, string con)
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(_coneccion);

                await conn.OpenAsync();

                const string sql = @"
        INSERT INTO roles
        (
            Nombre,
            Descripcion,
            PermisosId,
            Color,
            UserMail,
	Contrasena
        )
        VALUES
        (
            @Nombre,
            @Descripcion,
            @PermisosId,
            @Color,
            @UserMail{
            @Contrasena
        )";

                using MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.Add("@Nombre", MySqlDbType.VarChar).Value = nombre;
                cmd.Parameters.Add("@Descripcion", MySqlDbType.VarChar).Value = descripcion;
                cmd.Parameters.Add("@PermisosId", MySqlDbType.VarChar).Value = permisosId;
                cmd.Parameters.Add("@Color", MySqlDbType.Int32).Value = 0;
                cmd.Parameters.Add("@UserMail", MySqlDbType.VarChar).Value = userMail;
                cmd.Parameters.Add("@Contrasena", MySqlDbType.VarChar).Value = con;

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        internal static async Task InsertarPermisos(
    string permisosId,
    bool venderProductos,
    bool eliminarProductos,
    bool agregarProductos,
    bool editarProductos,
    bool verClientes)
        {
            using MySqlConnection conn = new MySqlConnection(_coneccion);

            await conn.OpenAsync();

            const string sql = @"
INSERT INTO permisos
(
    PermisosId,
    VenderProductos,
    EliminarProductos,
    AgregarProductos,
    EditarProductos,
    VerClientes
)
VALUES
(
    @PermisosId,
    @VenderProductos,
    @EliminarProductos,
    @AgregarProductos,
    @EditarProductos,
    @VerClientes
)";

            using MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@PermisosId", permisosId);
            cmd.Parameters.AddWithValue("@VenderProductos", venderProductos);
            cmd.Parameters.AddWithValue("@EliminarProductos", eliminarProductos);
            cmd.Parameters.AddWithValue("@AgregarProductos", agregarProductos);
            cmd.Parameters.AddWithValue("@EditarProductos", editarProductos);
            cmd.Parameters.AddWithValue("@VerClientes", verClientes);

            await cmd.ExecuteNonQueryAsync();
        }

        internal static DataTable ObtenerRolesPorCorreo(string correo)
        {
            using MySqlConnection conn = new MySqlConnection(_coneccion);

            conn.Open();

            const string sql = @"
        SELECT *
        FROM roles
        WHERE UserMail = @UserMail";

            using MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@UserMail", correo);

            using MySqlDataAdapter da = new MySqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt;
        }

        internal static PermisosEntity? ObtenerPermisos(string permisosId)
        {
            using MySqlConnection conn = new MySqlConnection(_coneccion);

            conn.Open();

            const string sql = @"
        SELECT *
        FROM permisos
        WHERE PermisosId = @PermisosId
        LIMIT 1";

            using MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@PermisosId", permisosId);

            using MySqlDataReader reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return new PermisosEntity(
                Convert.ToBoolean(reader["VenderProductos"]),
                Convert.ToBoolean(reader["EliminarProductos"]),
                Convert.ToBoolean(reader["AgregarProductos"]),
                Convert.ToBoolean(reader["EditarProductos"]),
                Convert.ToBoolean(reader["VerClientes"])
            );
        }

        internal static bool EliminarRolDB(string permisosId)
        {
            using MySqlConnection conn = new MySqlConnection(_coneccion);

            conn.Open();

            const string sql = @"
        DELETE FROM roles
        WHERE PermisosId = @PermisosId";

            using MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@PermisosId", permisosId);

            return cmd.ExecuteNonQuery() > 0;
        }

        internal static bool EliminarPermisosDB(string permisosId)
        {
            using MySqlConnection conn = new MySqlConnection(_coneccion);

            conn.Open();

            const string sql = @"
        DELETE FROM permisos
        WHERE PermisosId = @PermisosId";

            using MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@PermisosId", permisosId);

            return cmd.ExecuteNonQuery() > 0;
        }
        #endregion

        #region Ventas Services
        internal static async Task<bool> RestarStockProducto(int cantidad, string id)
        {
            using MySqlConnection conn = new MySqlConnection(_coneccion);

            await conn.OpenAsync();

            const string sql = @"
UPDATE productos
SET Stock = Stock - @Cantidad
WHERE Id = @Id";

            using MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Cantidad", cantidad);
            cmd.Parameters.AddWithValue("@Id", id);

            int filas = await cmd.ExecuteNonQueryAsync();

            return filas > 0;
        }
        internal static async Task SumarVentasAyer(string correoUsuario, decimal monto,decimal ingresos)
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(_coneccion);

                await conn.OpenAsync();

                const string sql = @"
    INSERT INTO stats (
    correoUsuario,
    ProductosVendidosAyer,
    IngresoRecaudado
)
VALUES (
    @correoUsuario,
    @monto,
    @ingreso
)
ON DUPLICATE KEY UPDATE
    ProductosVendidosAyer = COALESCE(ProductosVendidosAyer,0) + @monto,
    IngresoRecaudado = COALESCE(IngresoRecaudado,0) + @ingreso";

                using MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@correoUsuario", correoUsuario);
                cmd.Parameters.AddWithValue("@monto", monto);
                cmd.Parameters.AddWithValue("@ingreso", ingresos);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Conexion de PC services
        internal static async Task RegistrarEquipo(string correoUsuario, string nombrePc)
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(_coneccion);

                await conn.OpenAsync();

                const string sql = @"
INSERT INTO maquinasConectadas (correoUsuario, nombrePc)
VALUES (@correoUsuario, @nombrePc)
ON DUPLICATE KEY UPDATE
nombrePc = VALUES(nombrePc)";

                using MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@correoUsuario", correoUsuario);
                cmd.Parameters.AddWithValue("@nombrePc", nombrePc);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        internal static void EliminarEquipo(string nombrePc)
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(_coneccion);

                conn.Open();

                const string sql = @"
        DELETE FROM maquinasConectadas
        WHERE nombrePc = @nombrePc";

                using MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@nombrePc", nombrePc);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        internal static async Task<ListaSimple<MaquinaConectadaEntity>> ObtenerEquiposConectados()
        {
            try
            {
                ListaSimple<MaquinaConectadaEntity> equipos = new();

                using MySqlConnection conn = new MySqlConnection(_coneccion);

                await conn.OpenAsync();

                const string sql = @"
SELECT correoUsuario, nombrePc
FROM maquinasConectadas";

                using MySqlCommand cmd = new MySqlCommand(sql, conn);

                using MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    equipos.InsertarCola(new MaquinaConectadaEntity
                    {
                        CorreoUsuario = reader["correoUsuario"]?.ToString() ?? string.Empty,
                        NombrePc = reader["nombrePc"]?.ToString() ?? string.Empty
                    });
                }

                return equipos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region VetasService
        internal static async Task GuardarVentaMySQL(
    string idVenta,
    string correoUsuario,
    int numeroTicket,
    DateTime fecha,
    decimal total)
        {
            try
            {
                await using MySqlConnection conn = new MySqlConnection(_coneccion);

                await conn.OpenAsync();

                const string sql = @"
            INSERT INTO ventas
            (IdVenta, CorreoUsuario, NumeroTicket, Fecha, Total)
            VALUES
            (@IdVenta, @CorreoUsuario, @NumeroTicket, @Fecha, @Total)";

                await using MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@IdVenta", idVenta);
                cmd.Parameters.AddWithValue("@CorreoUsuario", correoUsuario);
                cmd.Parameters.AddWithValue("@NumeroTicket", numeroTicket);
                cmd.Parameters.AddWithValue("@Fecha", fecha);
                cmd.Parameters.AddWithValue("@Total", total);

                await cmd.ExecuteNonQueryAsync();
            }
            catch
            {
                throw;
            }
        }

        internal static async Task GuardarDetalleVentaMySQL(
            string idDetalle,
            string idVenta,
            string idProducto,
            string nombreProducto,
            int cantidad,
            decimal precioUnitario,
            decimal subtotal)
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(_coneccion);

                await conn.OpenAsync();

                const string sql = @"
INSERT INTO detalleVentas
(IdDetalle, IdVenta, IdProducto, NombreProducto, Cantidad, PrecioUnitario, Subtotal)
VALUES
(@IdDetalle, @IdVenta, @IdProducto, @NombreProducto, @Cantidad, @PrecioUnitario, @Subtotal)";

                using MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@IdDetalle", idDetalle);
                cmd.Parameters.AddWithValue("@IdVenta", idVenta);
                cmd.Parameters.AddWithValue("@IdProducto", idProducto);
                cmd.Parameters.AddWithValue("@NombreProducto", nombreProducto);
                cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                cmd.Parameters.AddWithValue("@PrecioUnitario", precioUnitario);
                cmd.Parameters.AddWithValue("@Subtotal", subtotal);

                await cmd.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        internal static async Task<int> ObtenerUltimoNumeroTicket()
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(_coneccion);

                await conn.OpenAsync();

                const string sql = @"
                SELECT MAX(NumeroTicket)
                FROM ventas";

                using MySqlCommand cmd = new MySqlCommand(sql, conn);

                object? resultado = await cmd.ExecuteScalarAsync();

                if (resultado == null || resultado == DBNull.Value)
                    return 0;

                return Convert.ToInt32(resultado);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public static async Task<ListaSimple<TicketResumen>> ObtenerVentasHoy(string correo)
        {
            ListaSimple<TicketResumen> lista = new ListaSimple<TicketResumen>();

            try
            {
                using MySqlConnection conn = new MySqlConnection(_coneccion);

                await conn.OpenAsync();

                DateTime inicio = DateTime.Today;
                DateTime fin = inicio.AddDays(1);

                const string sql = @"
    SELECT IdVenta, NumeroTicket, Fecha, Total
    FROM ventas
    WHERE CorreoUsuario = @Correo
    AND Fecha >= @Inicio
    AND Fecha < @Fin";

                using MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.Parameters.AddWithValue("@Inicio", inicio);
                cmd.Parameters.AddWithValue("@Fin", fin);

                using MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    lista.InsertarCola(new TicketResumen
                    {
                        IdVenta = reader["IdVenta"].ToString(),
                        NumeroTicket = Convert.ToInt32(reader["NumeroTicket"]),
                        Fecha = Convert.ToDateTime(reader["Fecha"]).ToString("yyyy-MM-dd HH:mm:ss"),
                        Total = Convert.ToDecimal(reader["Total"])
                    });
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public static async Task<decimal> ObtenerIngresoTotal()
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(_coneccion);

                await conn.OpenAsync();

                const string sql = @"
                SELECT SUM(Total)
                FROM ventas";

                using MySqlCommand cmd = new MySqlCommand(sql, conn);

                object result = await cmd.ExecuteScalarAsync();

                if (result == null || result == DBNull.Value)
                    return 0;

                return Convert.ToDecimal(result);
            }
            catch
            {
                return 0;
            }
        }

        public static async Task<decimal> ObtenerIngresosHoy()
        {
            try
            {
                string hoy = DateTime.Now.ToString("yyyy-MM-dd");

                using MySqlConnection conn = new MySqlConnection(_coneccion);

                await conn.OpenAsync();

                const string sql = @"
SELECT SUM(Total)
FROM ventas
WHERE Fecha LIKE @Fecha";

                using MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Fecha", hoy + "%");

                object result = await cmd.ExecuteScalarAsync();

                if (result == null || result == DBNull.Value)
                    return 0;

                return Convert.ToDecimal(result);
            }
            catch
            {
                return 0;
            }
        }

        public static async Task<ListaSimple<ProductosIngreso>?> ObtenerProductosMasRentables()
        {
            try
            {
                ListaSimple<ProductosIngreso> lista = new ListaSimple<ProductosIngreso>();

                using MySqlConnection conn = new MySqlConnection(_coneccion);

                await conn.OpenAsync();

                const string sql = @"
SELECT NombreProducto, SUM(Subtotal) AS Ingreso
FROM DetalleVentas
GROUP BY NombreProducto
ORDER BY Ingreso DESC";

                using MySqlCommand cmd = new MySqlCommand(sql, conn);

                using MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    lista.InsertarCola(new ProductosIngreso
                    {
                        NombreProducto = reader["NombreProducto"].ToString(),
                        Ingreso = Convert.ToDecimal(reader["Ingreso"])
                    });
                }

                return lista;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<ListaSimple<TicketResumen>?> ObtenerVentasPorFecha(DateTime inicio, DateTime fin)
        {
            try
            {
                ListaSimple<TicketResumen> lista = new ListaSimple<TicketResumen>();

                using MySqlConnection conn = new MySqlConnection(_coneccion);

                await conn.OpenAsync();

                const string sql = @"
SELECT *
FROM ventas
WHERE Fecha >= @Inicio AND Fecha < @Fin
ORDER BY Fecha DESC";

                using MySqlCommand cmd = new MySqlCommand(sql, conn);


                cmd.Parameters.AddWithValue("@Inicio", inicio.Date);
                cmd.Parameters.AddWithValue("@Fin", fin.Date.AddDays(1));

                using MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    lista.InsertarCola(new TicketResumen
                    {
                        IdVenta = reader["IdVenta"].ToString(),
                        NumeroTicket = Convert.ToInt32(reader["NumeroTicket"]),
                        Fecha = reader["Fecha"].ToString(),
                        Total = Convert.ToDecimal(reader["Total"])
                    });
                }

                return lista;
            }
            catch
            {
                return null;
            }
        }
        public static async Task<ListaSimple<ProductosIngreso>?> ObtenerProductosPorFecha(DateTime inicio, DateTime fin)
        {
            try
            {
                ListaSimple<ProductosIngreso> lista = new ListaSimple<ProductosIngreso>();

                using MySqlConnection conn = new MySqlConnection(_coneccion);

                await conn.OpenAsync();

                const string sql = @"
SELECT D.NombreProducto, SUM(D.Subtotal) AS Ingreso
FROM DetalleVentas D
INNER JOIN Ventas V ON D.IdVenta = V.IdVenta
WHERE V.Fecha >= @Inicio AND V.Fecha < @Fin
GROUP BY D.NombreProducto
ORDER BY Ingreso DESC";

                using MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Inicio", inicio.Date);
                cmd.Parameters.AddWithValue("@Fin", fin.Date.AddDays(1));

                using MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    lista.InsertarCola(new ProductosIngreso
                    {
                        NombreProducto = reader["NombreProducto"].ToString(),
                        Ingreso = Convert.ToDecimal(reader["Ingreso"])
                    });
                }

                return lista;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<decimal> ObtenerIngresoPorFecha(DateTime inicio, DateTime fin)
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(_coneccion);

                await conn.OpenAsync();

                const string sql = @"
SELECT SUM(Total)
FROM Ventas
WHERE Fecha >= @Inicio AND Fecha <= @Fin";

                using MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Inicio", inicio.Date);              
                cmd.Parameters.AddWithValue("@Fin", fin.Date.AddDays(1).AddTicks(-1)); 

                object result = await cmd.ExecuteScalarAsync();

                if (result == null || result == DBNull.Value)
                    return 0;

                return Convert.ToDecimal(result);
            }
            catch
            {
                return 0;
            }
        }
        public static async Task<DataTable?> ObtenerDetalleTicket(string idVenta)
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(_coneccion);

                await conn.OpenAsync();

                const string sql = @"
    SELECT *
    FROM detalleVentas
    WHERE IdVenta = @Id";

                using MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Id", idVenta);

                using MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                DataTable dt = new DataTable();
                dt.Load(reader);

                return dt;
            }
            catch
            {
                return null;
            }
        }


        public static async Task<ListaSimple<TicketResumen>?> ObtenerTodasLasVentas()
        {
            try
            {
                ListaSimple<TicketResumen> lista = new ListaSimple<TicketResumen>();

                using MySqlConnection conn = new MySqlConnection(_coneccion);

                await conn.OpenAsync();

                const string sql = @"
SELECT *
FROM ventas
ORDER BY Fecha DESC";

                using MySqlCommand cmd = new MySqlCommand(sql, conn);

                // No hay parámetros, pero lo dejo por consistencia si tu wrapper lo usa
                using MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                DataTable dt = new DataTable();
                dt.Load(reader);

                foreach (DataRow fila in dt.Rows)
                {
                    lista.InsertarCola(new TicketResumen
                    {
                        IdVenta = fila["IdVenta"].ToString(),
                        NumeroTicket = Convert.ToInt32(fila["NumeroTicket"]),
                        Fecha = fila["Fecha"].ToString(),
                        Total = Convert.ToDecimal(fila["Total"])
                    });
                }

                return lista;
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
