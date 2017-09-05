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
                MessageBox.Show("Proporcione Almacén", "SIGESTA", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (ddlEmpleado.SelectedValue == null)
            {
                MessageBox.Show("Proporcione Empleado", "SIGESTA", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    MessageBox.Show("Proporcione Cantidad", "SIGESTA", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtCodigo.Text = string.Empty;
                    txtCantidad.Text = string.Empty;
                    txtCantidad.Focus();
                    return;
                }

                AgregarItem((sender as TextBox).Text);
                Add.IsChecked = true;
                Del.IsChecked = false;
                txtCantidad.Text = "1";
                (sender as TextBox).Text = string.Empty;
                (sender as TextBox).Focus();
            }
        }
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCantidad.Text.Trim()))
            {
                MessageBox.Show("Proporcione Cantidad", "SIGESTA", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtCantidad.Text = string.Empty;
                txtCodigo.Text = string.Empty;
                txtCantidad.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtCodigo.Text.Trim()))
            {
                MessageBox.Show("Proporcione Código", "SIGESTA", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtCodigo.Text = string.Empty;
                txtCodigo.Focus();
                return;
            }

            if (txtCodigo.Text.Trim().Length != 16)
            {   
                MessageBox.Show("Código debe ser de 16 dígitos", "SIGESTA", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtCodigo.Text = string.Empty;
                txtCodigo.Focus();
                return;
            }

            AgregarItem(txtCodigo.Text.Trim());
            Add.IsChecked = true;
            Del.IsChecked = false;
            txtCantidad.Text = "1";
            txtCodigo.Text = string.Empty;
            txtCodigo.Focus();
        }
        private void AgregarItem(string codigo)
        {
            try
            {
                int i = Convert.ToInt32(txtCantidad.Text.Trim()) * (Add.IsChecked.Value ? 1 : Del.IsChecked.Value ? -1 : 1);

                LecturasViewModel vm = (LecturasViewModel)this.DataContext;
                ObservableCollection<LecturasModel> lecturas = vm.LecturasList;

                if (!string.IsNullOrEmpty(codigo))
                {
                    var encontrado = vm.LecturasList.Where(l => l.Codigo == codigo).FirstOrDefault();

                    if (encontrado != null)
                    {
                        encontrado.Total += i;

                        if (encontrado.Total <= 0)
                        {
                            vm.LecturasList.Remove(encontrado);
                        }
                    }
                    else
                    {
                        if (i <= 0)
                        {
                            MessageBox.Show("No hay elementos que quitar", "SIGESTA", MessageBoxButton.OK, MessageBoxImage.Warning);
                            txtCodigo.Text = string.Empty;
                            txtCantidad.Text = string.Empty;
                            txtCantidad.Focus();
                            return;
                        }

                        LecturasModel newLectura = new LecturasModel()
                        {
                            Almacen = ((ComboBoxItem)ddlAlmacen.SelectedItem).Content.ToString(),
                            Codigo = codigo,
                            Nombre = string.Format("Producto {0}", seqProduct),
                            ProductoId = seqProduct,
                            Total = i,
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
                string pref = string.Empty;
                string almacen = ((ComboBoxItem)ddlAlmacen.SelectedItem).Content.ToString();

                switch (almacen)
                {
                    case "REVENTON_1":
                        pref = "R1";
                        break;
                    case "REVENTON_2":
                        pref = "R2";
                        break;
                    case "REVENTON_3":
                        pref = "R3";
                        break;
                    case "REVENTON_4":
                        pref = "R4";
                        break;
                    case "REVENTON_5":
                        pref = "R5";
                        break;
                    case "REVENTON_6":
                        pref = "R6";
                        break;
                    case "REVENTON_7":
                        pref = "R7";
                        break;
                    case "REVENTON_8":
                        pref = "R8";
                        break;
                    default:
                        pref = "RX";
                        break;
                }

                string baseDirectory = @"C:\Escaneos";

                if (!Directory.Exists(baseDirectory))
                {
                   Directory.CreateDirectory(baseDirectory);
                }

                string appPath = baseDirectory + @"\" + pref + DateTime.Now.ToString("ddMMyymm") + ".txt";

                using (StreamWriter writer = new StreamWriter(appPath , true))
                {
                    foreach (LecturasModel lectura in lecturas)
                    {
                        if (!lectura.Equals(lecturas[lecturas.Count - 1]))
                        {
                            writer.WriteLine(string.Format("{0},{1}", lectura.Codigo, lectura.Total));
                        }
                        else
                        {
                            writer.Write(string.Format("{0},{1}", lectura.Codigo, lectura.Total));
                        }
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