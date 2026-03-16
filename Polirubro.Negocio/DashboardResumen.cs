using System;

namespace Polirubro.Negocio
{
    public class DashboardResumen
    {
        public decimal VentasHoy { get; set; }
        public decimal VentasMes { get; set; }
        public decimal VentasAnio { get; set; }
        public int CantidadVentasHoy { get; set; }
        public int CantidadVentasMes { get; set; }
        public int CantidadVentasAnio { get; set; }
        public string ProductoMasVendido { get; set; }
        public int CantidadProductoMasVendido { get; set; }
    }
}