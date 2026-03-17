using Polirubro.Datos;
using Polirubro.Entidades;
using System.Linq;
using System.Windows;

namespace PolirubroWPF
{
    public partial class FrmArticulo : Window
    {
        private int? articuloId = null;
        private bool soloLectura = false;

        public FrmArticulo()
        {
            InitializeComponent();
            CargarCategorias();
            Loaded += (s, e) => txtCodigoProducto.Focus();
        }

        public FrmArticulo(int idArticulo, bool modoSoloLectura = false) : this()
        {
            articuloId = idArticulo;
            soloLectura = modoSoloLectura;

            CargarArticulo();

            if (soloLectura)
            {
                txtCodigoProducto.IsReadOnly = true;
                txtNombre.IsReadOnly = true;
                txtDescripcion.IsReadOnly = true;
                txtPrecio.IsReadOnly = true;
                txtCosto.IsReadOnly = true;
                txtStock.IsReadOnly = true;
                txtStockMinimo.IsReadOnly = true;
                cmbCategoria.IsEnabled = false;

                btnGuardar.Visibility = Visibility.Collapsed;
                btnCancelar.Content = "OK";
            }
        }

        private void CargarCategorias()
        {
            using (var db = new PolirubroDbContext())
            {
                var categorias = db.Categorias.ToList();
                cmbCategoria.ItemsSource = categorias;
                cmbCategoria.DisplayMemberPath = "Nombre";
                cmbCategoria.SelectedValuePath = "Id";
            }
        }

        private void CargarArticulo()
        {
            using (var db = new PolirubroDbContext())
            {
                var articulo = db.Articulos.FirstOrDefault(a => a.Id == articuloId);

                if (articulo != null)
                {
                    txtCodigoProducto.Text = articulo.CodigoProducto;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtPrecio.Text = articulo.Precio.ToString();
                    txtCosto.Text = articulo.Costo.ToString();
                    txtStock.Text = articulo.Stock.ToString();
                    txtStockMinimo.Text = articulo.StockMinimo.ToString();
                    cmbCategoria.SelectedValue = articulo.CategoriaId;
                }
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string codigoProducto = txtCodigoProducto.Text.Trim();
            string nombre = txtNombre.Text.Trim();
            string descripcion = txtDescripcion.Text.Trim();

            if (string.IsNullOrWhiteSpace(codigoProducto) ||
                string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(descripcion) ||
                string.IsNullOrWhiteSpace(txtPrecio.Text) ||
                string.IsNullOrWhiteSpace(txtCosto.Text) ||
                string.IsNullOrWhiteSpace(txtStock.Text) ||
                string.IsNullOrWhiteSpace(txtStockMinimo.Text) ||
                cmbCategoria.SelectedValue == null)
            {
                MessageBox.Show("Todos los campos son obligatorios.");
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                MessageBox.Show("El precio debe ser un número válido.");
                return;
            }

            if (!decimal.TryParse(txtCosto.Text, out decimal costo))
            {
                MessageBox.Show("El costo debe ser un número válido.");
                return;
            }

            if (!int.TryParse(txtStock.Text, out int stock))
            {
                MessageBox.Show("El stock debe ser un número válido.");
                return;
            }

            if (!int.TryParse(txtStockMinimo.Text, out int stockMinimo))
            {
                MessageBox.Show("El stock mínimo debe ser un número válido.");
                return;
            }

            using (var db = new PolirubroDbContext())
            {
                string codigoNormalizado = codigoProducto.ToLower();

                bool codigoExiste;

                if (articuloId.HasValue)
                {
                    codigoExiste = db.Articulos.Any(a => a.CodigoProducto.ToLower() == codigoNormalizado && a.Id != articuloId.Value);
                }
                else
                {
                    codigoExiste = db.Articulos.Any(a => a.CodigoProducto.ToLower() == codigoNormalizado);
                }

                if (codigoExiste)
                {
                    MessageBox.Show("Ya existe un artículo con ese código de producto.");
                    return;
                }

                Articulo articulo;

                if (articuloId.HasValue)
                {
                    articulo = db.Articulos.FirstOrDefault(a => a.Id == articuloId.Value);

                    if (articulo == null)
                    {
                        MessageBox.Show("No se encontró el artículo.");
                        return;
                    }
                }
                else
                {
                    articulo = new Articulo();
                    db.Articulos.Add(articulo);
                }

                articulo.CodigoProducto = codigoProducto;
                articulo.Nombre = nombre;
                articulo.Descripcion = descripcion;
                articulo.Precio = precio;
                articulo.Costo = costo;
                articulo.Stock = stock;
                articulo.StockMinimo = stockMinimo;
                articulo.CategoriaId = (int)cmbCategoria.SelectedValue;

                db.SaveChanges();
            }

            DialogResult = true;
            Close();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}