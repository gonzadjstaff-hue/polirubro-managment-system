using Polirubro.Datos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PolirubroWPF
{
    public partial class Ventas : Window
    {
        public Ventas()
        {
            InitializeComponent();
            CargarVentas();
        }

        private void CargarVentas()
        {
            using (var db = new PolirubroDbContext())
            {
                var ventas = db.Ventas
                    .Include(v => v.Detalles)
                    .OrderByDescending(v => v.Fecha)
                    .ToList()
                    .Select(v => new VentaListadoItem
                    {
                        Id = v.Id,
                        Fecha = v.Fecha,
                        ClienteMostrar = v.ConsumidorFinal ? "Consumidor Final" : v.NombreCliente,
                        TotalMostrar = v.Total.ToString("$ #,##0.00", CultureInfo.InvariantCulture)
                    })
                    .ToList();

                dgVentas.ItemsSource = ventas;
            }
        }

        private void BtnVer_Click(object sender, RoutedEventArgs e)
        {
            var boton = sender as Button;
            int idVenta = (int)boton.CommandParameter;

            FrmDetalleVenta frm = new FrmDetalleVenta(idVenta);
            frm.ShowDialog();
        }


        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    public class VentaListadoItem
    {
        public int Id { get; set; }
        public System.DateTime Fecha { get; set; }
        public string ClienteMostrar { get; set; }
        public string TotalMostrar { get; set; }
    }
}