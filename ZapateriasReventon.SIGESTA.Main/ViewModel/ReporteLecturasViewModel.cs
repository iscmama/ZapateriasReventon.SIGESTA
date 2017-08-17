using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZapateriasReventon.SIGESTA.Main.Model;
using ZapateriasReventon.SIGESTA.Main.Business;
using ZapateriasReventon.SIGESTA.Main.Data;

namespace ZapateriasReventon.SIGESTA.Main.ViewModel
{
    public class ReporteLecturasViewModel : INotifyPropertyChanged
    {
        private List<Reporte> _ReporteList;

        public ReporteLecturasViewModel()
        {
            using (SIGESTARepository rep = new SIGESTARepository())
            {
                _ReporteList = rep.ObtenerReporte();
            }
        }
        public List<Reporte> ReporteList
        {
            get { return _ReporteList; }
            set { _ReporteList = value; OnPropertyChanged("ReporteList"); }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged Members        
    }
}