using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WsNsftContabilidad.Business.Entities.SociosNegocio;

namespace WsNsftContabilidad.Business.Entities.Asientos
{
    [DataContract(Namespace = "http://WsNsftContabilidad")]
    public class AsientoDetalle
    {
        [DataMember]
        public int transIDDetalle { get; set; }
        [DataMember]
        public int Line_IDDetalle { get; set; }
        [DataMember]
        public string AccountCode { get; set; }
        [DataMember]
        public double Debit { get; set; }
        [DataMember]
        public double Credit { get; set; }
        [DataMember]
        public string Reference1 { get; set; }
        [DataMember]
        public string Reference2 { get; set; }
        [DataMember]
        public string Reference3 { get; set; }
        [DataMember]
        public string ProjectCode { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public string CodigoRetencion { get; set; }
        [DataMember]
        public double BaseRetencion { get; set; }
        [DataMember]
        public double PorcentaRetenciones { get; set; }
        [DataMember]        
        public SocioNegocio socioNegocio { get; set; }

        
        public AsientoDetalle()
        {
            
            this.ShortName = string.Empty;
            this.AccountCode = string.Empty;
            this.Reference1 = string.Empty;
            this.Reference2 = string.Empty;
            this.Reference3 = string.Empty;
            this.ProjectCode = string.Empty;
            this.CodigoRetencion = string.Empty;
            socioNegocio = new SocioNegocio();
        }
    }
}
