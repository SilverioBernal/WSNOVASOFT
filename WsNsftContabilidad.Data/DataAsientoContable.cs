using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;
using WsNsftContabilidad.Business.Entities.Seguridad;
using WsNsftContabilidad.Business.Entities.Asientos;
using WsNsftContabilidad.Business.Entities;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace WsNsftContabilidad.Data
{
    public class DataAsientoContable
    {
        /// <summary>
        /// Objeto que proporciona actividades de conexion a Base de Datos.
        /// </summary>
        //private  ConexionBD conexionBD;
        private Database baseDatos;
        #region Constructor
        /// <summary>
        /// contructor de la clase
        /// </summary>
        public DataAsientoContable()
        {
            //this.conexionBD = ConexionBD.Conexion;
            this.baseDatos = DatabaseFactory.CreateDatabase("SAP");
        }        
        #endregion contructor

        /// <summary>
        /// metodo que realiza la creación de los asientos
        /// </summary>
        /// <param name="unDocumento"></param>
        /// <param name="unDocumentoLineas"></param>
        /// <param name="unTipoDocumento"></param>
        /// <returns></returns>
        public int CrearAsiento(Asiento unAsiento)
        {
            JournalEntries miAsientoContable;
            int miResultado = -1;
            // se crea el encabezado del asiento
            miAsientoContable = (JournalEntries)ConexionSAP.Conexion.compania.GetBusinessObject(BoObjectTypes.oJournalEntries);
            miAsientoContable.TransactionCode = unAsiento.codigoTransaccion;
            if (unAsiento.Memo.ToString().Length > 50)
                miAsientoContable.Memo = unAsiento.Memo.ToString().Substring(0, 49);
            else
                miAsientoContable.Memo = unAsiento.Memo.ToString();
            // se recorren las lineas                   
            foreach (AsientoDetalle linea in unAsiento.lineas)
            {
                if (linea.ShortName != null)
                {
                    miAsientoContable.Lines.ShortName = linea.ShortName;
                }
                miAsientoContable.Lines.AccountCode = linea.AccountCode;
                miAsientoContable.Lines.Debit = linea.Debit;
                miAsientoContable.Lines.Credit = linea.Credit;
                miAsientoContable.Reference = linea.Reference3 == null ? "" : linea.Reference3;
                miAsientoContable.Lines.Reference1 = linea.Reference1 == null ? "" : linea.Reference1;
                miAsientoContable.Lines.Reference2 = linea.Reference2 == null ? "" : linea.Reference2;
                miAsientoContable.Lines.AdditionalReference = linea.Reference3 == null ? "" : linea.Reference3;
                miAsientoContable.Lines.Add();
            }
            //se agrega el asiento contable
            miResultado = miAsientoContable.Add();
            if (miResultado != 0)
                throw new SAPException(miResultado, ConexionSAP.Conexion.compania.GetLastErrorDescription());
            else
                return Convert.ToInt32(ConexionSAP.Conexion.compania.GetNewObjectKey());
        }

    }
}
