using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using ZapateriasReventon.SIGESTA.Main.Model;
using System.Collections.ObjectModel;

namespace ZapateriasReventon.SIGESTA.Main.ViewModel
{
    public class MarcasViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<MarcasModel> _MarcasList;

        public MarcasViewModel()
        {
            //_MarcasList = new List<MarcasModel>()
            //{
            //    new MarcasModel() {MarcaId = 0, Codigo = "000", Marca = "Andrea0", FechaAlta = DateTime.Now, FechaModificacion = null }
            //};

            _MarcasList = new ObservableCollection<MarcasModel>();
        }
        public ObservableCollection<MarcasModel> MarcasList
        {
            get { return _MarcasList; }
            set { _MarcasList = value; OnPropertyChanged("MarcasList"); }
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