using System;
using System.Collections.Generic;
using System.Text;

namespace App.BackEnd.Domain.Entities
{
    public class VentasEntity
    {
        public ProductoEntity Plato { get; set; }
        public int Cantidad { get; set; }
        public VentasEntity(ProductoEntity plato)
        {
            Plato = plato;
            Cantidad = 1;
        }
    }
}
