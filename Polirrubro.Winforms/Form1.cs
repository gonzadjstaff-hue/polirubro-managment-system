using Polirubro.Datos;
using Microsoft.EntityFrameworkCore;
using Polirubro.Winforms;

namespace Polirrubro.Winforms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CargarArticulos();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void CargarArticulos()
        {
            using (var db = new PolirubroDbContext())
            {
                var lista = db.Articulos
                    .Include(a => a.Categoria)
                    .Select(a => new
                    {
                        a.Id,
                        a.CodigoProducto,
                        a.Nombre,
                        a.Descripcion,
                        a.Precio,
                        a.Costo,
                        a.Stock,
                        Categoria = a.Categoria.Nombre
                    })
                    .ToList();

                dataGridView1.DataSource = lista;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnNuevoArticulo_Click(object sender, EventArgs e)
        {
            FrmArticulo frm = new FrmArticulo();
            frm.ShowDialog();

            CargarArticulos();
        }

        private void btnEliminarArticulo_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un artículo.");
                return;
            }
            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);

            var confirm = MessageBox.Show(
                "¿Está seguro de eliminar este artículo?", 
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
                );

            if (confirm != DialogResult.Yes)
                return;

            using (var db = new PolirubroDbContext())
            {
                var articulo = db.Articulos.Find(id);

                if (articulo != null)
                {
                    db.Articulos.Remove(articulo);
                    db.SaveChanges();
                    
                }

            }
                CargarArticulos();
        }
    }
}