using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace WsNsftContabilidad.Business.Entities.Asientos
{
    /// <summary>
    /// Entidad para la gestion de asientos contables
    /// </summary>
    [DataContract(Namespace = "http://WsNsftContabilidad")]
    public class Asiento
    {
        #region Atributos
        [DataMember]
        public DateTime RefDate { get; set; }
        [DataMember]
        public string Memo { get; set; }
        [DataMember]
        public string Ref1 { get; set; }
        [DataMember]
        public string Ref2 { get; set; }
        [DataMember]
        public string TransCode { get; set; }
        [DataMember]
        public string Project { get; set; }
        [DataMember]
        public DateTime? TaxDate { get; set; }
        [DataMember]
        public DateTime? VatDate { get; set; }                

        /*Control de cambios 2016-11-03*/
        [DataMember]
        public bool? StampTax { get; set; }

        [DataMember]
        public bool? AutoVat { get; set; }

        [DataMember]
        public string indicator { get; set; }

        [DataMember]
        public bool? AutoStorno { get; set; }

        [DataMember]
        public DateTime? StornoDate { get; set; }

        [DataMember]
        public int? series { get; set; }

        [DataMember]
        public bool? ReportEU { get; set; }

        [DataMember]
        public bool? Report347 { get; set; }

        [DataMember]
        public int? Location { get; set; }
       
        [DataMember]
        public long CreateBy { get; set; }

        [DataMember]
        public bool? BlockDunn { get; set; }

        [DataMember]
        public bool? AutoWT { get; set; }

        [DataMember]
        public bool? Corisptivi { get; set; }

        [DataMember]
        public List<AsientoDetalle> lineas { get; set; }
        #endregion

        #region Constructor
        public Asiento()
        {
            this.Memo = string.Empty;
            this.Ref1 = string.Empty;
            this.Ref2 = string.Empty;
            this.TransCode = string.Empty;
            this.Project = string.Empty;
            
            lineas = new List<AsientoDetalle>();
        }
        #endregion
    }
}
