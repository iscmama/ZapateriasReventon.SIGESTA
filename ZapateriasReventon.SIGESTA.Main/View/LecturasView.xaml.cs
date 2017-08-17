using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZapateriasReventon.SIGESTA.Main.Model;
using ZapateriasReventon.SIGESTA.Main.ViewModel;
using ZapateriasReventon.SIGESTA.Main.Data;

namespace ZapateriasReventon.SIGESTA.Main.View
{
    public partial class LecturasView : MetroWindow
    {
        private DateTime inicioEscaneo { get; set; }
        private int seqProduct { get; set; }
        public LecturasView()
        {
            InitializeComponent();
        }
        private void btnIniciar_Click(object sender, RoutedEventArgs e)
        {
            if (ddlAlmacen.SelectedValue == null)
            {
                MessageBox.Show("Proporcione Almacén", "SIGESTA");
                return;
            }

            if (ddlEmpleado.SelectedValue == null)
            {
                MessageBox.Show("Proporcione Empleado", "SIGESTA");
                return;
            }

            spCantidad.Visibility = Visibility.Visible;
            spCaptura.Visibility = Visibility.Visible;
            EscaneosGrid.Visibility = Visibility.Visible;
            dpAcciones.Visibility = Visibility.Visible;
            spAlmacen.Visibility = Visibility.Hidden;
            spEmpleado.Visibility = Visibility.Hidden;

            inicioEscaneo = DateTime.Now;
            seqProduct = 1;
            txtCantidad.Text = "1";
            txtCodigo.Focus();
        }
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (EscaneosGrid.Items.Count == 0)
                {
                    MessageBox.Show("Debe realizar al menos 1 lectura", "SIGESTA", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                LecturasViewModel vm = (LecturasViewModel)this.DataContext;
                List<LecturasModel> lecturas = vm.LecturasList.ToList();

                string folio = string.Empty;

                using (SIGESTARepository _repo = new SIGESTARepository())
                {
                    folio = _repo.AddLectura(lecturas);
                }

                if(string.IsNullOrEmpty(folio))
                {
                    MessageBox.Show("No se logro registrar la Lectura. Intente más tarde", "SIGESTA", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                ExportToExcel<LecturasModel, Lecturas> s = new ExportToExcel<LecturasModel, Lecturas>();
                s.dataToPrint = lecturas;
                s.GenerateReport();

                CreateTxt(lecturas);

                MessageBox.Show("La información se guardo de forma exitosa", "SIGESTA", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("No se logro guardar la información. Intente más tarde", "SIGESTA", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void txtCodigo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (string.IsNullOrEmpty(txtCantidad.Text.Trim()))
                {
                    MessageBox.Show("Proporcione Cantidad", "SIGESTA");
                    txtCodigo.Text = string.Empty;
                    txtCantidad.Text = string.Empty;
                    txtCantidad.Focus();
                    return;
                }

                AgregarItem((sender as TextBox).Text);
                (sender as TextBox).Text = string.Empty;
                (sender as TextBox).Focus();
            }
        }
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCantidad.Text.Trim()))
            {
                MessageBox.Show("Proporcione Cantidad", "SIGESTA");
                txtCantidad.Text = string.Empty;
                txtCodigo.Text = string.Empty;
                txtCantidad.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtCodigo.Text.Trim()))
            {
                MessageBox.Show("Proporcione Código", "SIGESTA");
                txtCodigo.Text = string.Empty;
                txtCodigo.Focus();
                return;
            }

            if (txtCodigo.Text.Trim().Length != 16)
            {   
                MessageBox.Show("Código debe ser de 16 dígitos", "SIGESTA");
                txtCodigo.Text = string.Empty;
                txtCodigo.Focus();
                return;
            }

            AgregarItem(txtCodigo.Text.Trim());
            txtCodigo.Text = string.Empty;
            txtCodigo.Focus();
        }
        private void AgregarItem(string codigo)
        {
            try
            {
                LecturasViewModel vm = (LecturasViewModel)this.DataContext;
                ObservableCollection<LecturasModel> lecturas = vm.LecturasList;

                if (!string.IsNullOrEmpty(codigo))
                {
                    var encontrado = vm.LecturasList.Where(l => l.Codigo == codigo).FirstOrDefault();

                    if (encontrado != null)
                    {
                        encontrado.Total+= Convert.ToInt32(txtCantidad.Text.Trim());
                    }
                    else
                    {
                        LecturasModel newLectura = new LecturasModel()
                        {
                            Almacen = ((ComboBoxItem)ddlAlmacen.SelectedItem).Content.ToString(),
                            Codigo = codigo,
                            Nombre = string.Format("Producto {0}", seqProduct),
                            ProductoId = seqProduct,
                            Total = Convert.ToInt32(txtCantidad.Text.Trim()),
                            Fecha = DateTime.Now
                        };

                        vm.LecturasList.Add(newLectura);
                        seqProduct++;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void CreateTxt(List<LecturasModel> lecturas)
        {
            try
            {
                string appPath = AppDomain.CurrentDomain.BaseDirectory + "\\" + ((ComboBoxItem)ddlAlmacen.SelectedItem).Content.ToString() + "_Inventario_" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".txt";

                using (StreamWriter writer = new StreamWriter(appPath , true))
                {
                    foreach (LecturasModel lectura in lecturas)
                    {
                        writer.WriteLine(string.Format("{0},{1}", lectura.Codigo, lectura.Total));
                    }

                    writer.Close();
                }

                System.Diagnostics.Process.Start("notepad.exe", appPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    public class Lecturas : List<LecturasModel> { }
}