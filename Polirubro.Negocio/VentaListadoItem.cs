using System;

namespace Polirubro.Negocio
{
    public class VentaListadoItem
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string ClienteMostrar { get; set; }
        public decimal Total { get; set; }
    }
}