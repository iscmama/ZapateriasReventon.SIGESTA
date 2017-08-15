using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZapateriasReventon.SIGESTA.Main.Model;

namespace ZapateriasReventon.SIGESTA.Main.ViewModel
{
    public class LecturasViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<LecturasModel> _LecturasList;
      
        public LecturasViewModel()
        {
            //_MarcasList = new List<MarcasModel>()
            //{
            //    new MarcasModel() {MarcaId = 0, Codigo = "000", Marca = "Andrea0", FechaAlta = DateTime.Now, FechaModificacion = null }
            //};

            _LecturasList = new ObservableCollection<LecturasModel>();
        }
        public ObservableCollection<LecturasModel> LecturasList
        {
            get { return _LecturasList; }
            set { _LecturasList = value; OnPropertyChanged("LecturasList"); }
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

        private ICommand mUpdater;
        public ICommand UpdateCommand
        {
            get
            {
                if (mUpdater == null)
                    mUpdater = new Updater();
                return mUpdater;
            }
            set
            {
                mUpdater = value;
            }
        }
        private class Updater : ICommand
        {
            #region ICommand Members

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {

            }

            #endregion
        }
    }
}