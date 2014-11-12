using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace WsNsftContabilidad.Business.Entities.Asientos
{
    [DataContract(Namespace = "http://WsNsftContabilidad")]
    public  class Asiento
    {
        [DataMember]
        public int transIDEncabezado { get; set; }
        [DataMember]
        public string codigoTransaccion { get; set; }
        [DataMember]
        public int BatchNumbEncabezado { get; set; }
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public string ProfitCode { get; set; }
        [DataMember]
        public string Proyect { get; set; }
        [DataMember]
        public string Memo { get; set; }
        [DataMember]
        public List<AsientoDetalle> lineas { get; set; }

        public Asiento()
        {
            this.ProfitCode = string.Empty;
            this.Proyect = string.Empty;
            this.Memo = string.Empty;
            this.codigoTransaccion = string.Empty;
            lineas = new List<AsientoDetalle>();
        }
    }
}
