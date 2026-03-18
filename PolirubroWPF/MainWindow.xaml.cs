using Microsoft.EntityFrameworkCore;
using Polirubro.Datos;
using Polirubro.Entidades;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace PolirubroWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                Activate();
                txtBuscar.Focus();

                if (Sesion.HayUsuarioLogueado)
                {
                    Title = $"Articulos - {Sesion.UsuarioActual.Nombre} ({(Sesion.UsuarioActual.EsAdmin ? "Admin" : "User")})";
                    txtUsuarioActual.Text = $"Usuario: {Sesion.UsuarioActual.Nombre} ({(Sesion.UsuarioActual.EsAdmin ? "Admin" : "User")})";
                }
                if (!Sesion.UsuarioActual.EsAdmin)
                {
                    btnNuevoArticulo.Visibility = Visibility.Collapsed;
                    btnVentas.Visibility = Visibility.Collapsed;
                    btnDashboard.Visibility = Visibility.Collapsed;

                    dgArticulos.Columns[3].Visibility = Visibility.Collapsed;
                    dgArticulos.Columns[7].Visibility = Visibility.Collapsed;
                    dgArticulos.Columns[8].Visibility = Visibility.Collapsed;
                }
            };


            using (var db = new PolirubroDbContext())
            {
                db.Database.EnsureCreated();
            }

            Loaded += (s, e) => txtBuscar.Focus();

            CargarArticulos();
        }

        private string NormalizarTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            string textoNormalizado = texto.Trim().ToLower().Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char c in textoNormalizado)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);

                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private void CargarArticulos(string filtro = "")
        {
            using (var db = new PolirubroDbContext())
            {
                var articulos = db.Articulos
                    .Include(a => a.Categoria)
                    .ToList();

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    string filtroNormalizado = NormalizarTexto(filtro);

                    var palabras = filtroNormalizado
                        .Split(' ', System.StringSplitOptions.RemoveEmptyEntries)
                        .Where(p => !string.IsNullOrWhiteSpace(p))
                        .ToList();

                    articulos = articulos
                        .Where(a =>
                        {
                            string textoBusqueda = NormalizarTexto(
                                $"{a.CodigoProducto} {a.Nombre} {a.Descripcion} {a.Categoria?.Nombre}"
                            );

                            return palabras.All(p => textoBusqueda.Contains(p));
                        })
                        .ToList();
                }

                dgArticulos.ItemsSource = articulos;

                ActualizarAlertaStock(articulos);
            }
        }

        private void ActualizarAlertaStock(List<Articulo> articulos)
        {
            int cantidadBajoStock = articulos.Count(a => a.StockMinimo > 0 && a.Stock <= a.StockMinimo);

            if (cantidadBajoStock > 0)
            {
                btnAlertaStock.Content = $"Atención: {cantidadBajoStock} producto(s) con stock bajo";
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
            if (!Sesion.UsuarioActual.EsAdmin)
            {
                MessageBox.Show("No tenés permisos para realizar esta acción.");
                return;
            }
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
            if (!Sesion.UsuarioActual.EsAdmin)
            {
                MessageBox.Show("No tenés permisos para ver esta sección.");
                return;
            }
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
            if (!Sesion.UsuarioActual.EsAdmin)
            {
                MessageBox.Show("No tenés permisos para editar.");
                return;
            }
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
            if (!Sesion.UsuarioActual.EsAdmin)
            {
                MessageBox.Show("No tenés permisos para eliminar articulos.");
                return;
            }
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
            if (!Sesion.UsuarioActual.EsAdmin)
            {
                MessageBox.Show("No tenés aceso al Dashboard.");
                return;
            }
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
        private void BtnCambiarUsuario_Click(object sender, RoutedEventArgs e)
        {
            Sesion.CerrarSesion();
            LoginWindow login = new LoginWindow();
            bool? resultado = login.ShowDialog();

            if (resultado == true)
            {
                MainWindow nuevaVentana = new MainWindow();
                Application.Current.MainWindow = nuevaVentana;
                nuevaVentana.Show();
                Close();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }
        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
