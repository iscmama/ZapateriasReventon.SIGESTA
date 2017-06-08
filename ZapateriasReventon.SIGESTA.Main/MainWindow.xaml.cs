using MahApps.Metro.Controls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZapateriasReventon.SIGESTA.Main.View;
using ZapateriasReventon.SIGESTA.Main.ViewModel;

namespace ZapateriasReventon.SIGESTA.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Marcas_Click(object sender, RoutedEventArgs e)
        {
            MarcasViewModel vm = new MarcasViewModel();

            MarcasView marcas = new MarcasView();
            marcas.DataContext = vm;
            marcas.ShowDialog();
        }

        private void Modelos_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Lectura_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
