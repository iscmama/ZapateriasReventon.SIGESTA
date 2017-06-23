using System;
using System.Collections.Generic;
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
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System.Data.OleDb;
using ZapateriasReventon.SIGESTA.Main.Model;
using ZapateriasReventon.SIGESTA.Main.ViewModel;

namespace ZapateriasReventon.SIGESTA.Main.View
{
    /// <summary>
    /// Interaction logic for MarcasView.xaml
    /// </summary>
    public partial class MarcasView : MetroWindow
    {
        public MarcasView()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OleDbConnection Conn;
            OleDbCommand Cmd;

            try
            {
                OpenFileDialog openfile = new OpenFileDialog();
                openfile.DefaultExt = ".xlsx";
                openfile.Filter = "(.xlsx)|*.xlsx";

                var browsefile = openfile.ShowDialog();

                if (browsefile == true)
                {
                    MarcasViewModel vm = (MarcasViewModel)this.DataContext;
                    txtFilePath.Text = openfile.FileName;
                    string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + txtFilePath.Text + ";Extended Properties=Excel 12.0;Persist Security Info=True";
                    Conn = new OleDbConnection(excelConnectionString);
                    Conn.Open();
                    Cmd = new OleDbCommand();
                    Cmd.Connection = Conn;
                    Cmd.CommandText = "Select * from [Sheet1$]";
                    var Reader = Cmd.ExecuteReader();
                    while (Reader.Read())
                    {
                        int marcaId = Convert.ToInt32(Reader["marcaId"]);
                        string codigo = Reader["codigo"].ToString();
                        string marca = Reader["marca"].ToString();

                        MarcasModel newMarca = new MarcasModel()
                        {
                            MarcaId = marcaId,
                            Codigo = codigo,
                            Marca = marca,
                            FechaAlta = DateTime.Now
                        };

                        vm.MarcasList.Add(newMarca);
                    }

                    Reader.Close();
                    Conn.Close();

                    //this.MarcasGrid.ItemsSource = vm.MarcasList;
                    //this.MarcasGrid.DataContext 
                    txtFilePath.Text = string.Empty;

                    if (vm.MarcasList.Count > 0)
                    {
                        vm.ShowActualizar = true;
                    }

                    MessageBox.Show("Carga Exitosa", "SIGESTA");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la carga. Detalle: " + ex.Message, "SIGESTA");
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            Reiniciar();
            MarcasViewModel vm = (MarcasViewModel)this.DataContext;
            vm.ShowAgregar = false;
            vm.ShowNuevo = true;
        }
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            Reiniciar();
            MarcasViewModel vm = (MarcasViewModel)this.DataContext;
            vm.ShowAgregar = true;
            vm.ShowNuevo = false;
        }
        private void Reiniciar()
        {
            txtMarcaId.Text = string.Empty;
            txtCodigo.Text = string.Empty;
            txtMarca.Text = string.Empty;
            txtFechaAlta.Text = string.Empty;
            txtFechaModificacion.Text = string.Empty;
        }
    }
}