

namespace Polirubro.Entidades
{
    public class Articulo
    {
        public int Id { get; set; }
        public string CodigoProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public decimal Costo { get; set; }
        public int Stock { get; set; }
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        }
    }

