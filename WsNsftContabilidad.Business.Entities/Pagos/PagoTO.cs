using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsNsftContabilidad.Business;
using WsNsftContabilidad.Business.Entities.SociosNegocio;
using WsNsftContabilidad.Business.Entities.Pagos; 



namespace WsNsftContabilidad.Business.Entities.Pagos
{
    public class PagoTO
    {
        public enum TipoPago
        {
            Normal = 1,
            Directo = 2,
            Innocor = 3,
            Especial = 4,
            LegalizacionesNormal = 5,
            LegalizacionesEspecial = 6,
            NormalEmpresarial = 7,
            NormalTerceros = 8,
            Consignacion = 9
        }
        public enum EnumMovimiento
        {
            Actual,
            Primero,
            Anterior,
            Siguiente,
            Ultimo
        }

        ///// <summary>
        ///// Código del Socio
        ///// </summary>
        ///public String Codigo;

        ///// <summary>
        ///// Tipo de pago a realizar
        ///// </summary>
        public TipoPago Tipo { set; get; }
        
        ///// <summary>
        ///// Código del cliente (Corresponde a la cédula o al NIT)
        ///// </summary>      
        public SocioNegocio Socio { set; get; }
        /// <summary>
        /// Identificacion del pago
        /// </summary>
        public int DocEntry { set; get; }
        /// <summary>
        /// Numero del pago
        /// </summary>
        public int DocNum { set; get; }
        /// <summary>
        /// Descripción del pago
        /// </summary>
        public string Descripcion { set; get; }
        /// <summary>
        /// Estado del pago
        /// </summary>
        public string Estado { set; get; }
        /// <summary>
        /// Tipo de pago
        /// </summary>
        public string IdTipoPago { set; get; }
        /// <summary>
        /// Comentarios
        /// </summary>
        public string Comentarios { set; get; }
        /// <summary>
        /// Fecha de creación del pago
        /// </summary>
        public DateTime Fecha { set; get; }
         //<summary>
         //Lista de remisiones de cobro
         //</summary>
        public List<RemisionCobro> RemisionesCobro { set; get; }
        ///// <summary>
        ///// Valor total del pago
        ///// </summary>
        public double Total { set; get; }
         //<summary>
         //Movimiento
         //</summary>
        public EnumMovimiento Movimiento { set; get; }
        /////// <summary>
        /////// Lista de medios de pago
        /////// </summary>
        public List<MedioDePago> MediosDePago { set; get; }
        /// <summary>
        /// Cuenta puente consignaciones
        /// </summary>
        public string CuentaPuente { set; get; }
        /// <summary>
        /// Nombre Cuenta puente consignaciones
        /// </summary>
        public string NombreCuentaPuente { set; get; }
        /// <summary>
        /// Lista de medios de pago
        /// </summary>
        ////public List<Saldo> Saldos { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        public PagoTO()
        {
            this.Socio = new SocioNegocio();
            ////this.RemisionesCobro = new List<RemisionCobro>();
            this.MediosDePago = new List<MedioDePago>();
            //this.Saldos = new List<Saldo>();
        }

        /// Lista de medios de pago
        /// </summary>
        public List<Saldo> Saldos { set; get; }

        /// <summary>
        /// Constructor con el tipo de pago
        /// </summary>
        /// <param name="unTipo"></param>
        public PagoTO(string unTipo)
        {
            this.Socio = new SocioNegocio();
            //this.RemisionesCobro = new List<RemisionCobro>();
            this.MediosDePago = new List<MedioDePago>();
            //this.Saldos = new List<Saldo>();
            switch (unTipo)
            {
                case "Pago Normal":
                    this.Tipo = PagoTO.TipoPago.Normal;
                    break;
                case "Pago Directo":
                    this.Tipo = PagoTO.TipoPago.Directo;
                    break;
                case "Pago Innocor":
                    this.Tipo = PagoTO.TipoPago.Innocor;
                    break;
                case "Pago Especial/Otros Recaudos":
                    this.Tipo = PagoTO.TipoPago.Especial;
                    break;
                case "Pago Normal Empresarial":
                    this.Tipo = PagoTO.TipoPago.NormalEmpresarial;
                    break;
                case "Pago Normal Terceros":
                    this.Tipo = PagoTO.TipoPago.NormalTerceros;
                    break;
            }
        }
    }
}
