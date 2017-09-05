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
                string pref = string.Empty;
                string almacen = lecturas.FirstOrDefault().Almacen;

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

                using (StreamWriter writer = new StreamWriter(appPath, true))
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
}