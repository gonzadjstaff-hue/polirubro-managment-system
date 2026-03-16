

namespace Polirubro.Entidades
{
    public class VentaDetalle
    {
        public int Id { get; set; }
        public int VentaId { get; set; }
        
        public Venta Venta { get; set; }
        public int ArticuloId { get; set; }
        public Articulo Articulo { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
         public decimal Subtotal { get; set; }
    }
}
