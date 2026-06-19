using System.Windows.Media.Imaging;

namespace App.Domain.Entities
{
    public class TicketResumen
    {
        public string IdVenta { get; set; }

        public int NumeroTicket { get; set; }

        public string Fecha { get; set; }

        public decimal Total { get; set; }

        public BitmapImage Foto64 { get; set; }
        
    }
}
