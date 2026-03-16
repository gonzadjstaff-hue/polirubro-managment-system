using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Polirubro.Datos;
using Polirubro.Entidades;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using ClosedXML.Excel;


namespace PolirubroWPF
{
    public partial class FrmStockBajo : Window
    {
        private List<Articulo> articulosBajoStock = new List<Articulo>();

        public FrmStockBajo()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            using (var db = new PolirubroDbContext())
            {
                articulosBajoStock = db.Articulos
                    .Include(a => a.Categoria)
                    .Where(a => a.StockMinimo > 0 && a.Stock <= a.StockMinimo)
                    .OrderBy(a => a.Stock)
                    .ToList();

                dgStockBajo.ItemsSource = articulosBajoStock;
            }
        }

        private void BtnExportar_Click(object sender, RoutedEventArgs e)
        {
            if (articulosBajoStock == null || articulosBajoStock.Count == 0)
            {
                MessageBox.Show("No hay productos para exportar.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivo Excel (*.xlsx)|*.xlsx";
            saveFileDialog.FileName = "stock_bajo.xlsx";

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var workbook = new ClosedXML.Excel.XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Stock Bajo");

                    worksheet.Cell(1, 1).Value = "Código";
                    worksheet.Cell(1, 2).Value = "Nombre";
                    worksheet.Cell(1, 3).Value = "Categoría";
                    worksheet.Cell(1, 4).Value = "Stock actual";
                    worksheet.Cell(1, 5).Value = "Stock mínimo";

                    for (int i = 0; i < articulosBajoStock.Count; i++)
                    {
                        var articulo = articulosBajoStock[i];

                        worksheet.Cell(i + 2, 1).Value = articulo.CodigoProducto;
                        worksheet.Cell(i + 2, 2).Value = articulo.Nombre;
                        worksheet.Cell(i + 2, 3).Value = articulo.Categoria?.Nombre ?? "";
                        worksheet.Cell(i + 2, 4).Value = articulo.Stock;
                        worksheet.Cell(i + 2, 5).Value = articulo.StockMinimo;
                    }

                    var rango = worksheet.Range(1, 1, articulosBajoStock.Count + 1, 5);

                    var tabla = rango.CreateTable();
                    tabla.Theme = ClosedXML.Excel.XLTableTheme.TableStyleMedium9;

                    worksheet.Row(1).Style.Font.Bold = true;

                    worksheet.Columns().AdjustToContents();

                    worksheet.SheetView.FreezeRows(1);

                    workbook.SaveAs(saveFileDialog.FileName);
                }

                MessageBox.Show("Archivo Excel exportado correctamente.");
            }
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}