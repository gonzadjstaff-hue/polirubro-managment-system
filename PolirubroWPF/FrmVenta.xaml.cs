using Polirubro.Datos;
using Polirubro.Entidades;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PolirubroWPF
{
    public partial class FrmVenta : Window
    {
        private List<VentaDetalle> detalles = new List<VentaDetalle>();

        public FrmVenta()
        {
            InitializeComponent();
            dpFecha.SelectedDate = DateTime.Now;
            dgDetalleVenta.ItemsSource = detalles;
            chkConsumidorFinal.IsChecked = true;
            txtCantidadAgregar.Text = "1";
            ActualizarEstadoCliente();
            ActualizarTotal();
            txtCodigoArticulo.Focus();
        }

        private void chkConsumidorFinal_Checked(object sender, RoutedEventArgs e)
        {
            ActualizarEstadoCliente();
        }

        private void ActualizarEstadoCliente()
        {
            bool consumidorFinal = chkConsumidorFinal.IsChecked == true;

            txtNombreCliente.IsEnabled = !consumidorFinal;
            txtDocumentoCliente.IsEnabled = !consumidorFinal;

            if (consumidorFinal)
            {
                txtNombreCliente.Text = "";
                txtDocumentoCliente.Text = "";
            }
        }

        private void btnAgregarArticulo_Click(object sender, RoutedEventArgs e)
        {
            AgregarArticulo(false);
        }

        private void txtCodigoArticulo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AgregarArticulo(true);
            }
        }

        private void txtNombreArticulo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AgregarArticulo(false);
            }
        }

        private void AgregarArticulo(bool esEscaneo)
        {
            string codigo = txtCodigoArticulo.Text.Trim();
            string nombre = txtNombreArticulo.Text.Trim();

            if (string.IsNullOrWhiteSpace(codigo) && string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("Ingresá un código o un nombre de artículo.");
                return;
            }

            int cantidad = 1;

            if (!esEscaneo)
            {
                if (!int.TryParse(txtCantidadAgregar.Text.Trim(), out cantidad) || cantidad <= 0)
                {
                    MessageBox.Show("La cantidad debe ser un número mayor a cero.");
                    return;
                }
            }

            using (var db = new PolirubroDbContext())
            {
                Articulo articulo = null;

                if (!string.IsNullOrWhiteSpace(codigo))
                {
                    articulo = db.Articulos.FirstOrDefault(a => a.CodigoProducto.ToLower() == codigo.ToLower());
                }
                else
                {
                    articulo = db.Articulos.FirstOrDefault(a => a.Nombre.ToLower().Contains(nombre.ToLower()));
                }

                if (articulo == null)
                {
                    MessageBox.Show("No se encontró el artículo.");
                    return;
                }

                var detalleExistente = detalles.FirstOrDefault(d => d.ArticuloId == articulo.Id);

                if (detalleExistente != null)
                {
                    int nuevaCantidad = detalleExistente.Cantidad + cantidad;

                    if (nuevaCantidad > articulo.Stock)
                    {
                        MessageBox.Show($"Stock insuficiente para el artículo {articulo.Nombre}.");
                        return;
                    }

                    detalleExistente.Cantidad = nuevaCantidad;
                    detalleExistente.Subtotal = detalleExistente.Cantidad * detalleExistente.PrecioUnitario;
                }
                else
                {
                    if (cantidad > articulo.Stock)
                    {
                        MessageBox.Show($"Stock insuficiente para el artículo {articulo.Nombre}.");
                        return;
                    }

                    var nuevoDetalle = new VentaDetalle
                    {
                        ArticuloId = articulo.Id,
                        Articulo = articulo,
                        Cantidad = cantidad,
                        PrecioUnitario = articulo.Precio,
                        Subtotal = articulo.Precio * cantidad
                    };

                    detalles.Add(nuevoDetalle);
                }
            }

            RefrescarGrilla();
            ActualizarTotal();
            LimpiarIngresoArticulo(esEscaneo);
        }

        private void LimpiarIngresoArticulo(bool esEscaneo)
        {
            txtCodigoArticulo.Text = "";
            txtNombreArticulo.Text = "";

            if (esEscaneo)
            {
                txtCantidadAgregar.Text = "1";
            }

            txtCodigoArticulo.Focus();
        }

        private void btnQuitarArticulo_Click(object sender, RoutedEventArgs e)
        {
            if (dgDetalleVenta.SelectedItem == null)
            {
                MessageBox.Show("Seleccioná un artículo del detalle.");
                return;
            }

            var detalleSeleccionado = (VentaDetalle)dgDetalleVenta.SelectedItem;
            detalles.Remove(detalleSeleccionado);
            RefrescarGrilla();
            ActualizarTotal();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (dpFecha.SelectedDate == null)
            {
                MessageBox.Show("Debés seleccionar una fecha.");
                return;
            }

            if (detalles.Count == 0)
            {
                MessageBox.Show("Debés agregar al menos un artículo a la venta.");
                return;
            }

            bool consumidorFinal = chkConsumidorFinal.IsChecked == true;

            if (!consumidorFinal)
            {
                if (string.IsNullOrWhiteSpace(txtNombreCliente.Text) || string.IsNullOrWhiteSpace(txtDocumentoCliente.Text))
                {
                    MessageBox.Show("Debés completar nombre y documento del cliente.");
                    return;
                }
            }

            using (var db = new PolirubroDbContext())
            {
                foreach (var detalle in detalles)
                {
                    var articuloDb = db.Articulos.FirstOrDefault(a => a.Id == detalle.ArticuloId);

                    if (articuloDb == null)
                    {
                        MessageBox.Show($"No se encontró el artículo con ID {detalle.ArticuloId}.");
                        return;
                    }

                    if (articuloDb.Stock < detalle.Cantidad)
                    {
                        MessageBox.Show($"Stock insuficiente para el artículo {articuloDb.Nombre}.");
                        return;
                    }
                }

                var venta = new Venta
                {
                    Fecha = dpFecha.SelectedDate.Value,
                    ConsumidorFinal = consumidorFinal,
                    NombreCliente = consumidorFinal ? null : txtNombreCliente.Text,
                    DocumentoCliente = consumidorFinal ? null : txtDocumentoCliente.Text,
                    Total = detalles.Sum(d => d.Subtotal)
                };

                db.Ventas.Add(venta);
                db.SaveChanges();

                foreach (var detalle in detalles)
                {
                    var articuloDb = db.Articulos.First(a => a.Id == detalle.ArticuloId);

                    var nuevoDetalle = new VentaDetalle
                    {
                        VentaId = venta.Id,
                        ArticuloId = detalle.ArticuloId,
                        Cantidad = detalle.Cantidad,
                        PrecioUnitario = detalle.PrecioUnitario,
                        Subtotal = detalle.Subtotal
                    };

                    db.VentaDetalles.Add(nuevoDetalle);
                    articuloDb.Stock -= detalle.Cantidad;
                }

                db.SaveChanges();
            }

            MessageBox.Show("Venta guardada correctamente.");
            DialogResult = true;
            Close();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RefrescarGrilla()
        {
            dgDetalleVenta.ItemsSource = null;
            dgDetalleVenta.ItemsSource = detalles;
        }

        private void ActualizarTotal()
        {
            decimal total = detalles.Sum(d => d.Subtotal);
            txtTotal.Text = total.ToString("$ #,##0.00", CultureInfo.InvariantCulture);
        }
    }
}