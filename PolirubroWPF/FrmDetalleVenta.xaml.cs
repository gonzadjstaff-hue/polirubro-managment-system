using Polirubro.Datos;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;

namespace PolirubroWPF
{
    public partial class FrmDetalleVenta : Window
    {
        private int ventaId;

        public FrmDetalleVenta(int idVenta)
        {
            InitializeComponent();
            ventaId = idVenta;
            CargarVenta();
        }

        private void CargarVenta()
        {
            using (var db = new PolirubroDbContext())
            {
                var venta = db.Ventas
                    .Include(v => v.Detalles)
                    .ThenInclude(d => d.Articulo)
                    .FirstOrDefault(v => v.Id == ventaId);

                if (venta == null)
                {
                    MessageBox.Show("Venta no encontrada.");
                    Close();
                    return;
                }

                txtIdVenta.Text = venta.Id.ToString();
                txtFecha.Text = venta.Fecha.ToShortDateString();
                txtTotal.Text = venta.Total.ToString("$ #,##0.00");

                dgDetalle.ItemsSource = venta.Detalles.ToList();
            }
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}