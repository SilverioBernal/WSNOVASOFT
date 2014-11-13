﻿using System;
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
    /// <summary>
    /// Clase para la gestion de asientos contables en SAP
    /// </summary>
    public class DataAsientoContable
    {
        #region Propiedades
        /// <summary>
        /// Objeto que proporciona actividades de conexion a Base de Datos.
        /// </summary>        
        private Database baseDatos; 
        #endregion

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

        #region Métodos
        /// <summary>
        /// metodo que realiza la creación de asientos contables
        /// </summary>
        /// <param name="unAsiento">Objeto de tipo asiento contable</param>
        /// <returns>Numero de aiento contable</returns>
        public int CrearAsiento(Asiento unAsiento)
        {
            JournalEntries miAsientoContable;
            int miResultado = -1;

            // se crea el encabezado del asiento
            miAsientoContable = (JournalEntries)ConexionSAP.Conexion.compania.GetBusinessObject(BoObjectTypes.oJournalEntries);
            miAsientoContable.TransactionCode = unAsiento.TransCode;

            if (unAsiento.Memo.ToString().Length > 50)
                miAsientoContable.Memo = unAsiento.Memo.ToString().Substring(0, 49);
            else
                miAsientoContable.Memo = unAsiento.Memo.ToString();

            // Adicion de detalle de asiento 
            foreach (AsientoDetalle linea in unAsiento.lineas)
            {
                miAsientoContable.Lines.AccountCode = linea.Account;
                miAsientoContable.Lines.Debit = linea.Debit;
                miAsientoContable.Lines.Credit = linea.Credit;
                miAsientoContable.Reference = unAsiento.Ref1;
                miAsientoContable.Lines.Reference1 = linea.Ref1 == null ? "" : linea.Ref1;
                miAsientoContable.Lines.Reference2 = linea.Ref2 == null ? "" : linea.Ref2;
                miAsientoContable.Lines.AdditionalReference = linea.Ref3Line == null ? "" : linea.Ref3Line;

                //UDF Values
                miAsientoContable.Lines.UserFields.Fields.Item("U_InfoCo01").Value = linea.U_InfoCo01;
                miAsientoContable.Lines.Add();
            }
            //Creacion del asiento contable
            miResultado = miAsientoContable.Add();
            if (miResultado != 0)
                throw new SAPException(miResultado, ConexionSAP.Conexion.compania.GetLastErrorDescription());
            else
                return Convert.ToInt32(ConexionSAP.Conexion.compania.GetNewObjectKey());
        } 
        #endregion
    }
}
