using Microsoft.EntityFrameworkCore;
using Polirubro.Entidades;

namespace Polirubro.Datos
{
    public class PolirubroDbContext : DbContext
    {
        public DbSet<Articulo> Articulos { get; set; }  
        public DbSet<Categoria> Categorias  { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=polirubro.db");
        }
    }
}
