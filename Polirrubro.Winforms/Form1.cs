using Polirubro.Datos;
using Microsoft.EntityFrameworkCore;

namespace Polirrubro.Winforms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void CargarArticulos()
        {
            using (var db = new PolirubroDbContext())
            {
                dataGridView1.DataSource = db.Articulos.Include(a => a.Categoria).ToList();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}