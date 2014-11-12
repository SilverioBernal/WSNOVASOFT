using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsNsftContabilidad.Business.Entities.Pagos
{
    public class GlobalesPagos
    {
        public static string User { get; set; }
        public static string Password { get; set; }
        public static string Compania { get; set; }
        public static List<InformacionPorPago> informacionPagos = new List<InformacionPorPago>();
    }
}
