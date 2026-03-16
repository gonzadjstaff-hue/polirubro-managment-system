using Polirubro.Negocio;
using System.Windows;

namespace PolirubroWPF
{
    public partial class FrmDashboard : Window
    {
        public FrmDashboard()
        {
            InitializeComponent();
            CargarDashboard();
        }

        private void CargarDashboard()
        {
            DashboardService service = new DashboardService();
            DashboardResumen resumen = service.ObtenerResumen();

            txtVentasHoy.Text = resumen.VentasHoy.ToString("C");
            txtVentasMes.Text = resumen.VentasMes.ToString("C");
            txtVentasAnio.Text = resumen.VentasAnio.ToString("C");
            txtCantidadVentasHoy.Text = resumen.CantidadVentasHoy.ToString();
            txtCantidadVentasMes.Text = resumen.CantidadVentasMes.ToString();
            txtCantidadVentasAnio.Text = resumen.CantidadVentasAnio.ToString();
            txtProductoMasVendido.Text = resumen.ProductoMasVendido;
            txtCantidadProductoMasVendido.Text = resumen.CantidadProductoMasVendido.ToString();
        }
        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

}