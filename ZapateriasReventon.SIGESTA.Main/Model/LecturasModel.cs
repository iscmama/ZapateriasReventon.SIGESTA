using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZapateriasReventon.SIGESTA.Main.Model
{
    public class LecturasModel : INotifyPropertyChanged
    {
        private int productoId;
        private string almacen;
        private string codigo;
        private string nombre;
        private int total;
        private DateTime fecha;

        public int ProductoId
        {
            get { return productoId; }
            set { productoId = value; OnPropertyChanged("productoId"); }
        }
        public string Almacen
        {
            get { return almacen; }
            set { almacen = value; OnPropertyChanged("almacen"); }
        }
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; OnPropertyChanged("nombre"); }
        }
        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; OnPropertyChanged("codigo"); }
        }
        public int Total
        {
            get { return total; }
            set { total = value; OnPropertyChanged("total"); }
        }
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; OnPropertyChanged("fecha"); }
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
