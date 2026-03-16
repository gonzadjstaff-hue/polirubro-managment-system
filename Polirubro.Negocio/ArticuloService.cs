using Microsoft.EntityFrameworkCore;
using Polirubro.Datos;
using Polirubro.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace Polirubro.Negocio
{
    public class ArticuloService
    {
        public List<Articulo> ObtenerArticulos(string filtro = "")
        {
            using var db = new PolirubroDbContext();

            var query = db.Articulos
                .Include(a => a.Categoria)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                query = query.Where(a => a.Nombre.Contains(filtro) || a.CodigoProducto.Contains(filtro));
            }

            return query.ToList();
        }

        public List<Articulo> ObtenerStockBajo(int stockMinimo = 5)
        {
            using var db = new PolirubroDbContext();

            return db.Articulos
                .Include(a => a.Categoria)
                .Where(a => a.Stock <= stockMinimo)
                .OrderBy(a => a.Stock)
                .ToList();
        }

        public Articulo ObtenerPorId(int id)
        {
            using var db = new PolirubroDbContext();

            return db.Articulos
                .Include(a => a.Categoria)
                .FirstOrDefault(a => a.Id == id);
        }
    }
}