namespace App.BackEnd.Domain.Entities
{
    public class ClienteEntity
    {
        public string Nombre { get; set; }
        public string ProductosComprados { get; set; }
        public double DineroEgresado { get; set; }

        public ClienteEntity(string nombre, string productosComprados, double dineroEgresado)
        {
            Nombre = nombre;
            ProductosComprados = productosComprados;
            DineroEgresado = dineroEgresado;
        }

        public override string ToString()
        {
            return $"Cliente: {Nombre}  |  Productos Comprados: {ProductosComprados}  |  Dinero Egresado: {DineroEgresado}";
        }
    }
}
