using App.BackEnd.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace App.BackEnd.Domain.Entities
{
    public class ProductoEntity
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public CategoriasProductosEnum Cartegoria { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public BitmapImage Foto64 { get; set; }

        public ProductoEntity(string id, string nombre, CategoriasProductosEnum cartegoria, decimal precio, int stock, BitmapImage foto64)
        {
            Id = id;
            Nombre = nombre;
            Cartegoria = cartegoria;
            Precio = precio;
            Stock = stock;
            Foto64 = foto64;
        }

        public override string ToString()
        {
            return $"{Nombre}  -  S/. {Precio:F2}  |  Stock: {Stock}";
        }
    }
}