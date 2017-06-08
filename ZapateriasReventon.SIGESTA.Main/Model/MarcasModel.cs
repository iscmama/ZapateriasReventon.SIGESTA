using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZapateriasReventon.SIGESTA.Main.Model
{
    public class MarcasModel : INotifyPropertyChanged
    {
        private int marcaId;
        private string codigo;
        private string marca;
        private DateTime fechaAlta;
        private DateTime? fechaModificacion;

        public int MarcaId
        {
            get { return marcaId; }
            set { marcaId = value;   OnPropertyChanged("marcaId");  }
        }
        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; OnPropertyChanged("codigo"); }
        }
        public string Marca
        {
            get { return marca; }
            set { marca = value; OnPropertyChanged("marca"); }
        }
        public DateTime FechaAlta
        {
            get { return fechaAlta; }
            set { fechaAlta = value; OnPropertyChanged("fechaAlta"); }
        }

        public DateTime? FechaModificacion
        {
            get { return fechaModificacion; }
            set { fechaModificacion = value; OnPropertyChanged("fechaModificacion"); }
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