using System;   
using System.Collections.Generic;   

namespace Polirubro.Entidades
{
    public class Venta
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public bool ConsumidorFinal { get; set; }
        public string? NombreCliente { get; set; }
        public string? DocumentoCliente { get; set; }

        public List<VentaDetalle> Detalles { get; set; } = new List<VentaDetalle>();
    }
}
