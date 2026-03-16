using Microsoft.EntityFrameworkCore;
using Polirubro.Datos;
using Polirubro.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PolirubroWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            using (var db = new PolirubroDbContext())
            {
                db.Database.EnsureCreated();
            }

            CargarArticulos();
        }

        private void CargarArticulos(string filtro = "")
        {
            using (var db = new PolirubroDbContext())
            {
                var query = db.Articulos
                    .Include(a => a.Categoria)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    query = query.Where(a => a.Nombre.Contains(filtro) || a.CodigoProducto.Contains(filtro));
                }

                var lista = query.ToList();

                dgArticulos.ItemsSource = lista;

                ActualizarAlertaStock(lista);
            }
        }

        private void ActualizarAlertaStock(List<Articulo> articulos)
        {
            bool hayStockBajo = articulos.Any(a => a.StockMinimo > 0 && a.Stock <= a.StockMinimo);

            if (hayStockBajo)
            {
                btnAlertaStock.Content = "Atención: hay productos con stock bajo";
                btnAlertaStock.Visibility = Visibility.Visible;
            }
            else
            {
                btnAlertaStock.Visibility = Visibility.Collapsed;
            }
        }

        private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            CargarArticulos(txtBuscar.Text);
        }

        private void BtnNuevoArticulo_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new FrmArticulo();
            var resultado = ventana.ShowDialog();

            if (resultado == true)
            {
                CargarArticulos(txtBuscar.Text);
            }
        }

        private void BtnVenta_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new FrmVenta();
            var resultado = ventana.ShowDialog();

            if (resultado == true)
            {
                CargarArticulos(txtBuscar.Text);
            }
        }

        private void BtnVentas_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new Ventas();
            ventana.ShowDialog();
            CargarArticulos(txtBuscar.Text);
        }

        private void BtnVer_Click(object sender, RoutedEventArgs e)
        {
            var boton = sender as Button;
            int id = (int)boton.CommandParameter;

            var ventana = new FrmArticulo(id, true);
            ventana.ShowDialog();
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var boton = sender as Button;
            int id = (int)boton.CommandParameter;

            var ventana = new FrmArticulo(id);
            var resultado = ventana.ShowDialog();

            if (resultado == true)
            {
                CargarArticulos(txtBuscar.Text);
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            var boton = sender as Button;
            int id = (int)boton.CommandParameter;

            var confirmacion = MessageBox.Show("¿Querés eliminar este artículo?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (confirmacion != MessageBoxResult.Yes)
                return;

            using (var db = new PolirubroDbContext())
            {
                var articulo = db.Articulos.FirstOrDefault(a => a.Id == id);

                if (articulo != null)
                {
                    db.Articulos.Remove(articulo);
                    db.SaveChanges();
                }
            }

            CargarArticulos(txtBuscar.Text);
        }

        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            FrmDashboard ventana = new FrmDashboard();
            ventana.ShowDialog();
        }

        private void BtnAlertaStock_Click(object sender, RoutedEventArgs e)
        {
            FrmStockBajo ventana = new FrmStockBajo();
            ventana.ShowDialog();

            CargarArticulos(txtBuscar.Text);
        }

        private void dgArticulos_LoadingRow(object sender, DataGridRowEventArgs e)
        {
        }

        private void dgArticulos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}