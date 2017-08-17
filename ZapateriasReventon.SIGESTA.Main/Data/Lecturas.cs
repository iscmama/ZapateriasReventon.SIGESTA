using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZapateriasReventon.SIGESTA.Main.Data
{
    [Table("Lecturas")]
    public class Lecturas
    {
        [Key]
        public int id { get; set; }
        public string folio { get; set; }
        public int productoId { get; set; }
        public string almacen { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public int total { get; set; }  
        public DateTime fecha { get; set; }
    }
}