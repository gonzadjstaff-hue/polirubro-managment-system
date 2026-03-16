using Microsoft.EntityFrameworkCore;
using Polirubro.Datos;
using System;
using System.Linq;

namespace Polirubro.Negocio
{
    public class DashboardService
    {
        public DashboardResumen ObtenerResumen()
        {
            using var db = new PolirubroDbContext();

            var hoy = DateTime.Today;
            var ahora = DateTime.Now;

            var ventasHoy = db.Ventas.Where(v => v.Fecha.Date == hoy);
            var ventasMes = db.Ventas.Where(v => v.Fecha.Month == ahora.Month && v.Fecha.Year == ahora.Year);
            var ventasAnio = db.Ventas.Where(v => v.Fecha.Year == ahora.Year);

            var masVendido = db.VentaDetalles
                .Include(vd => vd.Articulo)
                .AsEnumerable()
                .GroupBy(vd => vd.ArticuloId)
                .Select(g => new
                {
                    Nombre = g.First().Articulo.Nombre,
                    Cantidad = g.Sum(x => x.Cantidad)
                })
                .OrderByDescending(x => x.Cantidad)
                .FirstOrDefault();

            return new DashboardResumen
            {
                VentasHoy = ventasHoy.Any() ? ventasHoy.Sum(v => v.Total) : 0,
                VentasMes = ventasMes.Any() ? ventasMes.Sum(v => v.Total) : 0,
                VentasAnio = ventasAnio.Any() ? ventasAnio.Sum(v => v.Total) : 0,
                CantidadVentasHoy = ventasHoy.Count(),
                CantidadVentasMes = ventasMes.Count(),
                CantidadVentasAnio = ventasAnio.Count(),
                ProductoMasVendido = masVendido != null ? masVendido.Nombre : "Sin ventas",
                CantidadProductoMasVendido = masVendido != null ? masVendido.Cantidad : 0
            };
        }
    }
}