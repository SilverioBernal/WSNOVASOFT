using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsNsftContabilidad.Business.Entities.Pagos
{
    public class Saldo
    {
        public enum TipoDocumento
        {
            Factura,
            NotaDebito,
            Asiento
        }
        /// <summary>
        /// Enumeraciones para el tipo de pago
        /// </summary>        
        public enum TipoAbono
        {
            /// <summary>
            /// Total
            /// </summary>            
            Total,
            /// <summary>
            /// Parcial
            /// </summary>            
            Parcial,

        }
        /// <summary>
        /// Abono
        /// </summary>
        public TipoAbono Abono { set; get; }
        public int DocEntry { set; get; }
        public int LineId { set; get; }
        public TipoDocumento Tipo { set; get; }
        public double SaldoPendiente { set; get; }
        public double Valoraplicar { set; get; }
        public double Total { set; get; }
        public int Numero { set; get; }
        public DateTime Fecha { set; get; }
        public bool Seleccionado = true;
    }
}
