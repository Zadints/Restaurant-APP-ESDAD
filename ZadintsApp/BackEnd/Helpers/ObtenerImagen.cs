using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace App.BackEnd.Helpers
{
    public class Imagen
    {
        public static BitmapImage? ObtenerDesdeBase64(string base64)
        {
            if (string.IsNullOrWhiteSpace(base64)) return null;

            byte[] bytes = Convert.FromBase64String(base64);

            using (var ms = new MemoryStream(bytes))
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.StreamSource = ms;
                img.EndInit();
                img.Freeze(); 
                return img;
            }
        }

        public static string ConvertirABase64(string rutaImagen)
        {
            byte[] bytes = File.ReadAllBytes(rutaImagen);
            return Convert.ToBase64String(bytes);
        }
    }
}
