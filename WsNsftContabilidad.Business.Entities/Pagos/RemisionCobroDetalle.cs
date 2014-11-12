using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsNsftContabilidad.Business.Entities.Pagos
{
    public class RemisionCobroDetalle
    {
        public enum Tipo
        {
            Vlr_Com_Propia,
            Vlr_Gastos,
            Vlr_ICA_sobre_Com,
            VlrPrima,
            VlrIVA,
            xVlrIVAsobreCom
        }
        /// <summary>
        /// Enumeraciones para el tipo de pago
        /// </summary>        
        public enum enumComision
        {
            /// <summary>
            /// Con comisión
            /// </summary>            
            Y,
            /// <summary>
            /// Sin comisión
            /// </summary>            
            N
        }
        /// <summary>
        /// Enumeraciones para el tipo de pago
        /// </summary>        
        public enum Abono
        {
            /// <summary>
            /// Parcial
            /// </summary>            
            Parcial,
            /// <summary>
            /// Total
            /// </summary>            
            Total,
            /// <summary>
            /// Iva Poliza
            /// </summary>   
            IvaPoliza
        }
        /// <summary>
        ///Cuenta  Account - JDT1
        /// </summary>
        public string Cuenta { set; get; }
        /// <summary>
        ///Número de Línea
        /// </summary>
        public int LineId { set; get; }
        /// <summary>
        /// Tomador de la poliza ShortName - JDT1
        /// </summary>
        public string Tomador { set; get; }
        /// <summary>
        /// Valor de la Prima de la Poliza (Credit cuando la cuenta es 83050001)
        /// </summary>
        public double Crebito { set; get; }
        /// <summary>
        /// Valor de la Prima de la Poliza (Debito cuando la cuenta es 81950401)
        /// </summary>
        public double Debito { set; get; }
        /// <summary>
        /// Valor de la Prima de la Poliza (Credit cuando la cuenta es 83050001)
        /// </summary>
        public double BalDueCred { set; get; }
        /// <summary>
        /// Valor de la Prima de la Poliza (Debito cuando la cuenta es 81950401)
        /// </summary>
        public double BalDueDeb { set; get; }
        /// <summary>
        /// U_CSS_Type. Este valor corresponde al tipo de línea en el asiento
        /// Vlr. Com. Propia
        /// Vlr. Gastos
        /// Vlr. ICA sobre Com.
        /// Vlr. Prima
        /// Vlr. IVA
        /// x Vlr. IVA sobre Com.
        /// </summary>
        public Tipo TipoRegistro { set; get; }
        /// <summary>
        /// Total Poliza
        /// </summary>
        public double TotalPoliza { set; get; }
        /// <summary>
        /// Valor a aplicar
        /// </summary>
        public double ValorAplicar { set; get; }
    }
}
