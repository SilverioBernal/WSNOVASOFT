using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsNsftContabilidad.Business.Entities.Pagos
{
    public class Impuesto
    {
        public string Code { get; set; }
        public string Cuenta { get; set; }
        public double porcentaje { get; set; }
        public string Cuentacontrapartida { get; set; }
        public string CodeContra { get; set; }       
    }
}
