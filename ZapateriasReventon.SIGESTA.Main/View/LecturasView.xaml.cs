using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZapateriasReventon.SIGESTA.Main.Model;
using ZapateriasReventon.SIGESTA.Main.ViewModel;

namespace ZapateriasReventon.SIGESTA.Main.View
{
    /// <summary>
    /// Interaction logic for LecturasView.xaml
    /// </summary>
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
            if (string.IsNullOrEmpty(txtAlmacen.Text.Trim()))
            {
                MessageBox.Show("Proporcione Almacén", "SIGESTA");
                return;
            }

            if (string.IsNullOrEmpty(txtEmpleado.Text.Trim()))
            {
                MessageBox.Show("Proporcione Empleado", "SIGESTA");
                return;
            }

            spCaptura.Visibility = Visibility.Visible;
            EscaneosGrid.Visibility = Visibility.Visible;
            dpAcciones.Visibility = Visibility.Visible;
            spAlmacen.Visibility = Visibility.Hidden;
            spEmpleado.Visibility = Visibility.Hidden;

            inicioEscaneo = DateTime.Now;
            seqProduct = 1;
            txtCodigo.Focus();
        }
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void txtCodigo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AgregarItem((sender as TextBox).Text);
                (sender as TextBox).Text = string.Empty;
                (sender as TextBox).Focus();
            }
        }
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigo.Text.Trim()))
            {
                MessageBox.Show("Proporcione Código", "SIGESTA");
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
                        encontrado.Total++;
                    }
                    else
                    {
                        LecturasModel newLectura = new LecturasModel()
                        {
                            Almacen = txtAlmacen.Text,
                            Codigo = codigo,
                            Nombre = string.Format("Producto {0}", seqProduct),
                            ProductoId = seqProduct,
                            Total = 1,
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
    }
}