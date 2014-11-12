using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsNsftContabilidad.Business.Entities.Pagos
{
    public class MedioDePago
    {
        /// <summary>
        /// Enumeracion Tipo del medio de pago
        /// </summary>       
        public enum TipoMedioPago
        {
            /// <summary>
            /// Efectivo
            /// </summary>            
            Efectivo,
            /// <summary>
            /// Transferencia
            /// </summary>            
            Transferencia,
            /// <summary>
            /// Cheque
            /// </summary>            
            Cheque,
            /// <summary>
            /// Tarjeta
            /// </summary>            
            Tarjeta
        }
        /// <summary>
        /// Tipo del medio de pago
        /// </summary>
        public TipoMedioPago TipoMedio { get; set; }
        /// <summary>   
        /// Numero del cheque o la tarjeta
        /// </summary>
        public string NumeroChequeTarjeta { get; set; }
        /// <summary>
        /// Banco para el cheque
        /// </summary>
        public string Banco { get; set; }
        /// <summary>
        /// Numero de cuenta
        /// </summary>
        public string Cuenta { get; set; }
        /// <summary>
        /// Valor pagado con el medio de pago
        /// </summary>
        public double Valor { get; set; }
        /// <summary>
        /// Id tarjeta credito
        /// </summary>
        public string TarjetaCredito { get; set; }
        /// <summary>
        /// Tarjeta Valido Hasta
        /// </summary>
        public string ValidoHasta { get; set; }
        /// <summary>
        /// NumeroID
        /// </summary>
        public string NumeroID { get; set; }
        /// <summary>
        /// Numero de telefono
        /// </summary>
        public string NumeroTelefono { get; set; }
        /// <summary>
        /// Forma de pago
        /// </summary>
        public string FormaPago { get; set; }
        /// <summary>
        /// Importe Vencido
        /// </summary>
        public double ImporteVencido { get; set; }
        /// <summary>
        /// Numero de Voucher
        /// </summary>
        public string NumeroVoucher { get; set; }
    }
}
