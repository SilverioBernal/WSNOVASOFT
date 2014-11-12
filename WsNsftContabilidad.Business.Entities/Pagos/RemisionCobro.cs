using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsNsftContabilidad.Business.Entities.Pagos
{
    public class RemisionCobro
    {
        /// <summary>
        /// Enumeraciones para el tipo de pago
        /// </summary>        
        public enum SIoNO
        {
            /// <summary>
            /// SI
            /// </summary>            
            Y,
            /// <summary>
            /// NO
            /// </summary>            
            N
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
            /// <summary>
            /// IvaPoliza
            /// </summary>            
            IvaPoliza,
        }
        /// <summary>
        /// Abono
        /// </summary>
        public TipoAbono Abono { set; get; }
        /// <summary>
        /// Selecionado
        /// </summary>
        public bool Seleccionado { set; get; }
        /// <summary>
        /// ConComision
        /// </summary>
        public bool ConComision { set; get; }
        /// <summary>
        /// Id de la Remisión de cobro - OJDT - Ref3
        /// </summary>      
        public string NumeroRemisionCobro { set; get; }
        /// <summary>
        /// Fecha de creación -  
        /// </summary>
        public DateTime Fecha { set; get; }
        /// <summary>
        ///  Número del asiento en SAP - TransId OJDT
        /// </summary>      
        public int TransId { set; get; }
        /// <summary>
        /// Número Certificado - Ref1 OJDT
        /// </summary>   
        public string NumeroCertificado { set; get; }
        /// <summary>
        /// Número Poliza - Ref2 OJDT
        /// </summary>   
        public string NumeroPoliza { set; get; }
        /// <summary>
        /// Placa U_CSS_Placa OJDT
        /// </summary>
        public string Placa { set; get; }
        /// <summary>
        /// Placa U_CSS_Aseguradora OJDT
        /// </summary>
        public Aseguradora Aseguradora { set; get; }
        /// <summary>
        /// Placa U_CSS_Ramos JDT1
        /// </summary>
        public Ramo Ramo { set; get; }
        /// <summary>
        ///  Clase Ramos- JDT1 U_CSS_ClaseRamos
        /// </summary>
        public string ClaseRamos { set; get; }
        /// <summary>
        ///  Porcentaje de Comisión OJDT - U_CSS_PrcComision  
        /// </summary>
        public double PorcentajeComision { set; get; }
        /// <summary>
        ///  % participacion correcol OJDT - U_CSS_PrcParticCorre  
        /// </summary>
        public double PorcentajeParticipacionCorrecol { set; get; }
        /// <summary>
        /// Innocorr (SI/NO) - U_CSS_Innocorr OJDT
        /// </summary>
        public SIoNO Innocor { set; get; }
        /// <summary>
        /// Sumatoria de Prima, IVA y Gastos JDT1
        /// </summary>
        public double TotalPoliza { set; get; }
        /// <summary>
        /// Sumatoria de Prima, IVA y Gastos JDT1
        /// </summary>
        public double TotalPolizaCreditos { set; get; }
        /// <summary>
        /// Sumatoria de Prima, IVA y Gastos JDT1
        /// </summary>
        public double TotalPolizasDebitos { set; get; }
        /// <summary>
        /// Valor a aplicar
        /// </summary>
        public double Valoraplicar { set; get; }
        /// <summary>
        /// Valor del IVA
        /// </summary>
        public double IVA { set; get; }
        /// <summary>
        /// Valor de los gastos
        /// </summary>
        public double Gastos { set; get; }
        /// <summary>
        /// Valor de la prima
        /// </summary>
        public double Prima { set; get; }
        /// <summary>
        /// Valor Comisión Propia
        /// </summary>
        public double ComisionPropia { set; get; }
        /// <summary>
        /// Técnicos. U_CSS_Tecnicos
        /// </summary>
        public string Tecnicos { set; get; }
        /// <summary>
        /// Clase Comerciales. U_CSS_Comerciales
        /// </summary>
        public string Comerciales { set; get; }
        /// <summary>
        /// Empresarial U_CSS_Empresarial
        /// </summary>
        public string Empresarial { set; get; }
        /// <summary>
        /// Sector. U_CSS_Sector
        /// </summary>
        public string Sector { set; get; }
        /// <summary>
        /// Naturaleza Jurídica	U_CSS_NatJuridica
        /// </summary>
        public string NaturalezaJuridica { set; get; }
        /// <summary>
        /// Observaciones
        /// </summary>
        public string Observaciones { set; get; }
        /// <summary>
        /// Lista de Remisiones
        /// </summary>
        public List<RemisionCobroDetalle> RemisionCobroDetalle { set; get; }
        /// <summary>
        /// Comision
        /// </summary>
        public double Comision { set; get; }
        /// <summary>
        /// Nit Innocor
        /// </summary>
        public string NitInnocor { set; get; }
        /// <summary>
        /// Referencia3
        /// </summary>
        public string Referencia3 { set; get; }
        /// <summary>
        /// Porcentaje Agente
        /// </summary>
        public double PorcentajeAgente { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        public RemisionCobro()
        {
            this.Aseguradora = new Aseguradora();
            this.Ramo = new Ramo();
            RemisionCobroDetalle = new List<RemisionCobroDetalle>();
        }      
    }
}
