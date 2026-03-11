using Polirubro.Datos;
using Polirubro.Entidades;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Polirubro.Winforms
{
    public partial class FrmArticulo : Form
    {
        public FrmArticulo()
        {
            InitializeComponent();
            CargarCategotrias();
        }

        private void CargarCategotrias()
        {
            using (var db = new PolirubroDbContext())
            {
                var categorias = db.Categorias.ToList();

                cmbCategoria.DataSource = categorias;
                cmbCategoria.DisplayMember = "Nombre";
                cmbCategoria.ValueMember = "Id";
            }

        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigoProducto.Text) ||
                string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtDescripcion.Text) ||
                string.IsNullOrWhiteSpace(txtPrecio.Text) ||
                string.IsNullOrWhiteSpace(txtCosto.Text) ||
                string.IsNullOrWhiteSpace(txtStock.Text) ||
                cmbCategoria.SelectedItem == null)
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) ||
                !decimal.TryParse(txtCosto.Text, out decimal costo) ||
                !int.TryParse(txtStock.Text, out int stock))
            {
                MessageBox.Show("Precio, costo o stock inválidos.");
                return;
            }

            using (var db = new PolirubroDbContext())
            {
                Articulo articulo = new Articulo();

                articulo.CodigoProducto = txtCodigoProducto.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Precio = precio;
                articulo.Costo = costo;
                articulo.Stock = stock;
                articulo.CategoriaId = (int)cmbCategoria.SelectedValue;

                db.Articulos.Add(articulo);
                db.SaveChanges();
            }

            MessageBox.Show("Artículo guardado correctamente.");
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}
