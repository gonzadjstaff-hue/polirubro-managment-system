using Microsoft.EntityFrameworkCore;
using Polirubro.Entidades;

namespace Polirubro.Datos
{
    public class PolirubroDbContext : DbContext
    {
        public DbSet<Articulo> Articulos { get; set; }  
        public DbSet<Categoria> Categorias  { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=polirubro.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Nombre = "Libreía" },
                new Categoria { Id = 2, Nombre = "Tecnología" },
                new Categoria { Id = 3, Nombre = "Regalería" }
            );
            modelBuilder.Entity<Articulo>().HasData(
                new Articulo { Id = 1, CodigoProducto = "LIB001", Nombre = "lápiz Negro", Descripcion = "Poducto de prueba", Precio = 150, Costo = 70, Stock = 100, CategoriaId = 1 },
                new Articulo { Id = 2, CodigoProducto = "LIB002", Nombre = "Cuaderno A4", Descripcion = "Producto de prueba", Precio = 600, Costo = 300, Stock = 50, CategoriaId = 1 },
                new Articulo { Id = 3, CodigoProducto = "LIB003", Nombre = "Regla 30cm", Descripcion = "Producto de prueba", Precio = 200, Costo = 90, Stock = 200, CategoriaId = 1 },

                new Articulo { Id = 4, CodigoProducto = "TEC001", Nombre = "Mouse Inalámbrico", Descripcion = "Producto de prueba", Precio = 3000, Costo = 1500, Stock = 30, CategoriaId = 2 },
                new Articulo { Id = 5, CodigoProducto = "TEC002", Nombre = "Teclado Inalámbrico", Descripcion = "Producto de prueba", Precio = 8000, Costo = 4000, Stock = 20, CategoriaId = 2 },
                new Articulo { Id = 6, CodigoProducto = "TEC003", Nombre = "Auriculares Bluetooth", Descripcion = "Producto de prueba", Precio = 5000, Costo = 2500, Stock = 40, CategoriaId = 2 },

                new Articulo { Id = 7, CodigoProducto = "REG001", Nombre = "Taza Cerámica", Descripcion = "Producto de prueba", Precio = 8000, Costo = 400, Stock = 60, CategoriaId = 3 },
                new Articulo { Id = 8, CodigoProducto = "REG002", Nombre = "Llevaros Personalizados", Descripcion = "Producto de prueba", Precio = 8000, Costo = 500, Stock = 250, CategoriaId = 3 },
                new Articulo { Id = 9, CodigoProducto = "REG003", Nombre = "Agenda 2026", Descripcion = "Producto de prueba", Precio = 8000, Costo = 1200, Stock = 70, CategoriaId = 3 }
                );
        }
    }
}
