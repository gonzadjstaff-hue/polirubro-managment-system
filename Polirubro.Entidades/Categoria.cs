

namespace Polirubro.Entidades
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public List<Articulo> Articulos { get; set; }
    }
}
