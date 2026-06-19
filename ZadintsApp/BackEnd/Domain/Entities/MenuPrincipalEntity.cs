
namespace App.BackEnd.Domain.Entities
{
    public class MenuPrincipalEntity
    {
        public int? ProductosVendidosHoy { get; set; }
        public int? ProductosVendidosAyer { get; set; }
        public decimal? IngresoTotalHoy { get; set; }
        public decimal? IngresoRecaudado { get; set; }

        
        public MenuPrincipalEntity()
        {
            IngresoTotalHoy = 0;
            IngresoRecaudado = 0;
            ProductosVendidosAyer = 0;
            ProductosVendidosHoy = 0;


        }
        
    }
}
