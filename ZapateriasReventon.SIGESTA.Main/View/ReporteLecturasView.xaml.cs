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
    /// <summary>
    /// Interaction logic for ReporteLecturas.xaml
    /// </summary>
    public partial class ReporteLecturasView : MetroWindow
    {
        public ReporteLecturasView()
        {
            InitializeComponent();
        }

        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            List<LecturasModel> lecturas = new List<LecturasModel>();

            string folio = ((System.Windows.Controls.Primitives.ButtonBase)sender).CommandParameter.ToString();

            using (SIGESTARepository _repo = new SIGESTARepository())
            {
                lecturas = _repo.ObtenerLecturas(folio);
            }

            ExportToExcel<LecturasModel, Lecturas> s = new ExportToExcel<LecturasModel, Lecturas>();
            s.dataToPrint = lecturas;
            s.GenerateReport();

            CreateTxt(lecturas);

            MessageBox.Show("La información se exporto de forma exitosa", "SIGESTA", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void CreateTxt(List<LecturasModel> lecturas)
        {
            try
            {
                string appPath = AppDomain.CurrentDomain.BaseDirectory + "\\" + lecturas.First().Almacen + "_Inventario_" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".txt";

                using (StreamWriter writer = new StreamWriter(appPath, true))
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
}