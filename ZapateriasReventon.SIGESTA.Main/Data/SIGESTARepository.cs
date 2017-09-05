using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ZapateriasReventon.SIGESTA.Main.Model;
using ZapateriasReventon.SIGESTA.Main.Business;
using System.Data;

namespace ZapateriasReventon.SIGESTA.Main.Data
{
    public class SIGESTARepository : IDisposable
    {
        private SIGESTAContext _conCtx;
        public SIGESTARepository()
        {
            _conCtx = new SIGESTAContext();
        }
        public string AddLectura(List<LecturasModel> listaLecturas)
        {
            string folio = string.Empty;

            try
            {
                string pref = string.Empty;
                string almacen = listaLecturas.First().Almacen;

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

                folio = pref + DateTime.Now.ToString("ddMMyymm");

                foreach (LecturasModel l in listaLecturas)
                {
                    _conCtx.Lecturas.Add(
                         new Lecturas()
                         {
                             folio = folio,
                             productoId = l.ProductoId,
                             almacen = l.Almacen,
                             codigo = l.Codigo,
                             nombre = l.Nombre,
                             total = l.Total,
                             fecha = l.Fecha
                         }
                        );
                }

                _conCtx.SaveChanges();

                return folio;
            }
            catch (Exception)
            {
                folio = string.Empty;
            }

            return folio;
        }
        public List<Reporte> ObtenerReporte()
        {
            List<Reporte> reporte = new List<Reporte>();

            try
            {
                DataTable dtReporte = SQLHelper.ExecuteStoredProcedure("SP_REPORTE_LECTURAS");

                foreach (DataRow row in dtReporte.Rows)
                {
                    reporte.Add(new Reporte()
                    {
                        Folio = row["Folio"].ToString(),
                        Fecha = row["Fecha"].ToString(),
                        Total = Convert.ToInt32(row["Total"].ToString()),
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return reporte;
        }
        public List<LecturasModel> ObtenerLecturas(string folio)
        {
            try
            {
                var lecturas = (from l in _conCtx.Lecturas
                               where l.folio == folio
                               select new LecturasModel()
                               {
                                   ProductoId = l.productoId,
                                   Almacen = l.almacen,
                                   Nombre = l.nombre,
                                   Codigo = l.codigo,
                                   Total = l.total,
                                   Fecha = l.fecha
                               }
                         ).ToList();

                return lecturas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Dispose()
        {
            _conCtx.Dispose();
        }
    }
}