using Microsoft.EntityFrameworkCore;
using Polirubro.Datos;
using Polirubro.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace Polirubro.Negocio
{
    public class VentaService
    {
        public List<VentaListadoItem> ObtenerVentas()
        {
            using var db = new PolirubroDbContext();

            return db.Ventas
                .OrderByDescending(v => v.Fecha)
                .Select(v => new VentaListadoItem
                {
                    Id = v.Id,
                    Fecha = v.Fecha,
                    ClienteMostrar = v.ConsumidorFinal ? "Consumidor Final" : (v.NombreCliente ?? ""),
                    Total = v.Total
                })
                .ToList();
        }

        public Venta ObtenerVentaConDetalle(int ventaId)
        {
            using var db = new PolirubroDbContext();

            return db.Ventas
                .Include(v => v.Detalles)
                .ThenInclude(d => d.Articulo)
                .FirstOrDefault(v => v.Id == ventaId);
        }
    }
}