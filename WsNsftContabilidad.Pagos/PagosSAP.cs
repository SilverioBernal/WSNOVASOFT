using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Data;
using System.Configuration;
using SAPbobsCOM;
using WsNsftContabilidad.Business.Entities.Seguridad;
using WsNsftContabilidad.Business.Entities.Pagos;
using WsNsftContabilidad.Business.Entities; 
using System.Data.Common;


namespace WsNsftContabilidad.Data.Pagos
{
    public class PagosSAP
    {
        /// <summary>
        /// Objeto que proporciona conexion a la Base de Datos.
        /// </summary>
        private ConexionBD conexionBD;
        #region Constructor
        /// <summary>
        /// contructor de la clase
        /// </summary>
        public PagosSAP()
        {
            this.conexionBD = ConexionBD.Conexion;
        }
        #endregion contructor

        /// <summary>
        /// Permite generar un pago en SAP
        /// </summary>        
        /// <param name="unPago">Objeto con los datos del pago a realizar</param>
        /// <returns>Mensaje indicando el resultado del pago.</returns>
        public int RealizarPago(PagoTO unPago)
        {
            Payments miPago = (Payments)ConexionSAP.Conexion.compania.GetBusinessObject(BoObjectTypes.oIncomingPayments);
            miPago.DocTypte = BoRcptTypes.rCustomer;
            miPago.DocDate = unPago.Fecha;
            miPago.TaxDate = unPago.Fecha;
            miPago.CardCode = unPago.Socio.CardCode;
            miPago.Remarks = unPago.Comentarios;
            miPago.UserFields.Fields.Item("U_CSS_TipoPago").Value = unPago.Tipo.ToString();

            InformacionPorPago informacionPago = GlobalesPagos.informacionPagos.Find(X => X.Code.Equals(((int)unPago.Tipo).ToString()));
            if (informacionPago != null)
            {
                if (informacionPago.Serie.Length > 0)
                    miPago.Series = Convert.ToInt32(informacionPago.Serie);
                else
                    throw new SAPException(0, "No se ha definido una serie para el tipo de pago");
            }
            else
                throw new SAPException(0, "No se ha definido una serie para el tipo de pago");
            if (unPago.Tipo == PagoTO.TipoPago.Consignacion)
            {
                miPago.DocType = BoRcptTypes.rAccount;
                miPago.AccountPayments.AccountCode = unPago.CuentaPuente;
                miPago.AccountPayments.AccountName = unPago.NombreCuentaPuente;
                miPago.AccountPayments.SumPaid = unPago.Total;
                /// cambio de  unPago.Socio.codigo por unPago.Socio.CardCode
                miPago.AccountPayments.UserFields.Fields.Item("U_Tercero").Value = unPago.Socio.CardCode;
                miPago.AccountPayments.Add();
            }
            //miPago.Series = 1;//Serie  del pago           
            foreach (MedioDePago medio in unPago.MediosDePago)
            {
                switch (medio.TipoMedio)
                {
                    case MedioDePago.TipoMedioPago.Efectivo:
                        miPago.CashSum = medio.Valor;
                        miPago.CashAccount = medio.Cuenta;
                        break;
                    case MedioDePago.TipoMedioPago.Cheque:
                        miPago.Checks.BankCode = medio.Banco;
                        miPago.Checks.CheckNumber = Convert.ToInt32(medio.NumeroChequeTarjeta);
                        miPago.CheckAccount = medio.Cuenta;
                        miPago.Checks.CheckSum = medio.Valor;
                        miPago.Checks.Add();
                        break;
                    case MedioDePago.TipoMedioPago.Transferencia:
                        miPago.TransferAccount = medio.Cuenta;
                        miPago.TransferSum = medio.Valor;
                        break;
                    case MedioDePago.TipoMedioPago.Tarjeta:
                        miPago.CreditCards.CreditType = SAPbobsCOM.BoRcptCredTypes.cr_Regular;
                        miPago.CreditCards.CardValidUntil = Convert.ToDateTime(medio.ValidoHasta);
                        miPago.CreditCards.CreditAcct = medio.Cuenta;
                        miPago.CreditCards.CreditCardNumber = medio.NumeroChequeTarjeta.ToString();
                        miPago.CreditCards.CreditSum = medio.ImporteVencido;
                        miPago.CreditCards.VoucherNum = medio.NumeroVoucher;
                        miPago.CreditCards.OwnerIdNum = medio.NumeroID;
                        miPago.CreditCards.CreditCard = Convert.ToInt32(medio.TarjetaCredito);
                        miPago.CreditCards.OwnerPhone = medio.NumeroTelefono;
                        miPago.CreditCards.Add();
                        break;
                }
            }
            foreach (Saldo saldo in unPago.Saldos)
            {
                switch (saldo.Tipo)
                {
                    case Saldo.TipoDocumento.Asiento: miPago.Invoices.InvoiceType = BoRcptInvTypes.it_JournalEntry;
                        miPago.Invoices.DocLine = saldo.LineId;
                        break;
                    case Saldo.TipoDocumento.NotaDebito: miPago.Invoices.InvoiceType = BoRcptInvTypes.it_Invoice;
                        break;
                    case Saldo.TipoDocumento.Factura: miPago.Invoices.InvoiceType = BoRcptInvTypes.it_Invoice;
                        break;
                }
                miPago.Invoices.DocEntry = saldo.DocEntry;
                miPago.Invoices.SumApplied = saldo.Valoraplicar;
                miPago.Invoices.Add();
            }
            foreach (RemisionCobro remision in unPago.RemisionesCobro)
            {
                RemisionCobroDetalle Iva = remision.RemisionCobroDetalle.Find(x => x.TipoRegistro == RemisionCobroDetalle.Tipo.VlrIVA);
                if (Iva != null)
                {
                    miPago.Invoices.InvoiceType = BoRcptInvTypes.it_JournalEntry;
                    miPago.Invoices.DocLine = Iva.LineId;
                    miPago.Invoices.DocEntry = remision.TransId;
                    miPago.Invoices.SumApplied = remision.IVA;
                    miPago.Invoices.UserFields.Fields.Item("U_CSS_Comision").Value = remision.Comision;
                    miPago.Invoices.Add();
                }
                if (remision.Abono != RemisionCobro.TipoAbono.IvaPoliza)
                {
                    RemisionCobroDetalle Prima = remision.RemisionCobroDetalle.Find(x => x.TipoRegistro == RemisionCobroDetalle.Tipo.VlrPrima);
                    if (Prima != null)
                    {
                        miPago.Invoices.InvoiceType = BoRcptInvTypes.it_JournalEntry;
                        miPago.Invoices.DocLine = Prima.LineId;
                        miPago.Invoices.DocEntry = remision.TransId;
                        miPago.Invoices.SumApplied = remision.Prima;
                        miPago.Invoices.UserFields.Fields.Item("U_CSS_Comision").Value = remision.Comision;
                        miPago.Invoices.Add();
                    }
                    RemisionCobroDetalle Gastos = remision.RemisionCobroDetalle.Find(x => x.TipoRegistro == RemisionCobroDetalle.Tipo.Vlr_Gastos);
                    if (Gastos != null)
                    {
                        miPago.Invoices.InvoiceType = BoRcptInvTypes.it_JournalEntry;
                        miPago.Invoices.DocLine = Gastos.LineId;
                        miPago.Invoices.DocEntry = remision.TransId;
                        miPago.Invoices.SumApplied = remision.Gastos;
                        miPago.Invoices.UserFields.Fields.Item("U_CSS_Comision").Value = remision.Comision;
                        miPago.Invoices.Add();
                    }
                }
            }
            int miResultado = miPago.Add();
            if (miResultado != 0)
                throw new SAPException(miResultado, ConexionSAP.Conexion.compania.GetLastErrorDescription());
            else
                return Convert.ToInt32(ConexionSAP.Conexion.compania.GetNewObjectKey());
        }


        /////////// <summary>
        /////////// Recupera una consignacion
        /////////// </summary>        
        ////////public void ObtenerConsignacion(PagoTO unPago)
        ////////{
        ////////    StringBuilder Consulta = new StringBuilder();
        ////////    Consulta.Append("SELECT DISTINCT T0.DocNum, T1.AcctCode, T1.AcctName,T0.Comments,T3.BankCode,  ");
        ////////    Consulta.Append("T0.TrsfrAcct, T2.AcctName, T0.TrsfrSum, T4.U_InfoCo01  ");
        ////////    Consulta.Append("FROM ORCT T0  ");
        ////////    Consulta.Append("INNER JOIN RCT4 T1 ON T0.DocNum=T1.DocNum  ");
        ////////    Consulta.Append("INNER JOIN OACT T2 ON T2.AcctCode=T0.TrsfrAcct  ");
        ////////    Consulta.Append("INNER JOIN DSC1 T3 ON T3.GLAccount = T2.AcctCode  ");
        ////////    Consulta.Append("INNER JOIN JDT1 T4 ON T4.TransId=T0.TransId  ");
        ////////    Consulta.Append("WHERE T0.DocType='A' ");
        ////////    if (unPago.Movimiento == PagoTO.EnumMovimiento.Actual)
        ////////        Consulta.Append("AND T0.DocNum = @NumeroPago ");
        ////////    else if (unPago.Movimiento == PagoTO.EnumMovimiento.Primero)
        ////////        Consulta.Append("AND T0.DocNum = (SELECT MIN(DocNum) FROM ORCT WHERE DocType='A') ");
        ////////    else if (unPago.Movimiento == PagoTO.EnumMovimiento.Anterior)
        ////////        Consulta.Append("AND T0.DocNum = (SELECT TOP 1 DocNum FROM ORCT WHERE DocType='A' AND DocNum<@NumeroPago ORDER BY 1 DESC)");
        ////////    else if (unPago.Movimiento == PagoTO.EnumMovimiento.Siguiente)
        ////////        Consulta.Append("AND T0.DocNum = (SELECT TOP 1 DocNum FROM ORCT WHERE DocType='A' AND DocNum>@NumeroPago ORDER BY 1 ASC)");
        ////////    else if (unPago.Movimiento == PagoTO.EnumMovimiento.Ultimo)
        ////////        Consulta.Append("AND T0.DocNum = (SELECT MAX(DocNum) FROM ORCT WHERE DocType='A') ");
        ////////    DataSet miDataSet = new DataSet();
        ////////    DbCommand miComando = this.conexionBD.baseDatos.GetSqlStringCommand(Consulta.ToString());
        ////////    this.conexionBD.baseDatos.AddInParameter(miComando, "@NumeroPago", DbType.String, unPago.DocNum);
        ////////    miDataSet = this.conexionBD.baseDatos.ExecuteDataSet(miComando);
        ////////    DataTable miDataTable = new DataTable();
        ////////    miDataTable = miDataSet.Tables[0];
        ////////    if (miDataTable.Rows.Count > 0)
        ////////    {
        ////////        unPago.DocNum = Convert.ToInt32(miDataTable.Rows[0][0]);
        ////////        unPago.CuentaPuente = miDataTable.Rows[0][1].ToString();
        ////////        unPago.NombreCuentaPuente = miDataTable.Rows[0][2].ToString();
        ////////        unPago.Comentarios = miDataTable.Rows[0][3].ToString();
        ////////        unPago.Socio.Identificacion = miDataTable.Rows[0][8].ToString();
        ////////        MedioDePago medioPago = new MedioDePago();
        ////////        medioPago.Banco = miDataTable.Rows[0][4].ToString();
        ////////        medioPago.Cuenta = miDataTable.Rows[0][5].ToString();
        ////////        medioPago.Valor = Convert.ToDouble(miDataTable.Rows[0][7].ToString());
        ////////        unPago.MediosDePago.Add(medioPago);
        ////////    }
        ////////}
        /// <summary>
        /// Recupera un pago
        /// </summary>        
        public void ObtenerPagoEspecial(PagoTO unPago)
        {
            DateTime fecha = DateTime.Now;
            List<Cuenta> miListaCuentas = new List<Cuenta>(0);
            StringBuilder Consulta = new StringBuilder();
            InformacionPorPago infoPago = GlobalesPagos.informacionPagos.Find(x => (int)x.TipoDePago == (int)unPago.Tipo);
            Consulta.Append("SELECT T0.DocDate, T0.TaxDate, T0.CardCode, T0.CardName,  ");
            Consulta.Append("T0.CashSum, T0.CashAcct,  ");//--Efectivo
            Consulta.Append("T0.BankCode, T2.CheckNum, T0.CheckAcct, T0.CheckSum,  ");//--Cheque
            Consulta.Append("T0.TrsfrAcct, T0.TrsfrSum,   ");//--Transferencia
            Consulta.Append("CASE T1.InvType WHEN 13 THEN (SELECT DocType FROM OINV WHERE DocEntry = T1.DocEntry) ELSE 'Asiento' END 'Tipo', T1.DocLine, T1.DocEntry, T1.SumApplied, T0.DocTotal, T3.U_CSS_Type, T3.Credit, T1.SumApplied, T0.Comments, T0.DocNum, T4.Ref3, isnull (T1.U_CSS_Comision, 0), ");
            Consulta.Append("CASE T1.InvType WHEN 13 THEN (SELECT DocNum FROM OINV WHERE DocEntry = T1.DocEntry) ELSE T1.DocEntry END 'Numero', ");
            Consulta.Append("CASE T1.InvType WHEN 13 THEN (SELECT DocDate FROM OINV WHERE DocEntry = T1.DocEntry) ");
            Consulta.Append("ELSE (SELECT RefDate FROM OJDT WHERE TransId = T1.DocEntry) END 'Fecha', ");
            Consulta.Append("CASE T1.InvType WHEN 13 THEN (SELECT INV6.InsTotal FROM OINV INNER JOIN [INV6] ON  OINV.[DocEntry] = INV6.[DocEntry] AND OINV.DocEntry = T1.DocEntry) ");
            Consulta.Append("ELSE (SELECT Debit FROM OJDT WHERE TransId = T1.DocEntry) END 'Total', ");
            Consulta.Append("CASE T1.InvType WHEN 13 THEN (SELECT  INV6.InsTotal - INV6.PaidtoDate FROM OINV INNER JOIN [INV6] ON  OINV.[DocEntry] = INV6.[DocEntry] AND OINV.DocEntry = T1.DocEntry) ");
            Consulta.Append("ELSE (SELECT BalDueDeb FROM JDT1 WHERE TransId = T1.DocEntry AND Line_ID = DocLine) END 'Saldo' ");
            Consulta.Append("FROM ORCT T0 ");
            Consulta.Append("INNER JOIN RCT2 T1 ON T0.DocNum=T1.DocNum ");
            Consulta.Append("LEFT JOIN RCT1 T2 ON T2.DocNum=T0.DocNum ");
            Consulta.Append("INNER JOIN JDT1 T3 ON T3.Line_ID=T1.DocLine ");
            Consulta.Append("AND T3.TransId = T1.DocEntry ");
            Consulta.Append("INNER JOIN OJDT T4 ON T3.TransId=T4.TransId ");
            Consulta.Append("WHERE T0.DocType='C'  AND T0.Canceled = 'N' ");
            if (unPago.Movimiento == PagoTO.EnumMovimiento.Actual)
                Consulta.Append("AND T0.DocNum = @NumeroPago AND T0.Series = @Series AND U_CSS_TipoPago = 'Especial' ");
            else if (unPago.Movimiento == PagoTO.EnumMovimiento.Primero)
                Consulta.Append("AND T0.DocNum = (SELECT MIN(DocNum) FROM ORCT WHERE DocType='C' AND T0.Series = @Series AND U_CSS_TipoPago = 'Especial') ");
            else if (unPago.Movimiento == PagoTO.EnumMovimiento.Anterior)
                Consulta.Append("AND T0.DocNum = (SELECT TOP 1 DocNum FROM ORCT WHERE DocType='C'  AND T0.Series = @Series AND DocNum<@NumeroPago  AND U_CSS_TipoPago = 'Especial' ORDER BY 1 DESC)");
            else if (unPago.Movimiento == PagoTO.EnumMovimiento.Siguiente)
                Consulta.Append("AND T0.DocNum = (SELECT TOP 1 DocNum FROM ORCT WHERE DocType='C'  AND T0.Series = @Series AND DocNum>@NumeroPago AND U_CSS_TipoPago = 'Especial' ORDER BY 1 ASC)");
            else if (unPago.Movimiento == PagoTO.EnumMovimiento.Ultimo)
                Consulta.Append("AND T0.DocNum = (SELECT MAX(DocNum) FROM ORCT WHERE DocType='C'  AND T0.Series = @Series AND U_CSS_TipoPago = 'Especial') ");
            DataSet miDataSet = new DataSet();
            DbCommand miComando = this.conexionBD.baseDatos.GetSqlStringCommand(Consulta.ToString());
            this.conexionBD.baseDatos.AddInParameter(miComando, "@NumeroPago", DbType.String, unPago.DocNum);
            this.conexionBD.baseDatos.AddInParameter(miComando, "@Series", DbType.String, infoPago.Serie);
            miDataSet = this.conexionBD.baseDatos.ExecuteDataSet(miComando);
            DataTable miDataTable = new DataTable();
            miDataTable = miDataSet.Tables[0];
            if (miDataTable.Rows.Count > 0)
            {
                //Encabezado
                unPago.Fecha = Convert.ToDateTime(miDataTable.Rows[0][0]);
                unPago.Socio.CardCode  = miDataTable.Rows[0][2].ToString();
                unPago.Socio.CardName  = miDataTable.Rows[0][3].ToString();
                unPago.Total = Convert.ToDouble(miDataTable.Rows[0][16].ToString());
                unPago.Comentarios = miDataTable.Rows[0][20].ToString();
                unPago.DocNum = Convert.ToInt32(miDataTable.Rows[0][21].ToString());
                //Medios de pago
                //Efectivo
                MedioDePago medioPago = new MedioDePago();
                if (Convert.ToDouble(miDataTable.Rows[0][4].ToString()) > 0)
                {
                    medioPago.TipoMedio = MedioDePago.TipoMedioPago.Efectivo;
                    medioPago.Valor = Convert.ToDouble(miDataTable.Rows[0][4].ToString());
                    medioPago.Cuenta = miDataTable.Rows[0][5].ToString();
                    unPago.MediosDePago.Add(medioPago);
                }
                //Cheque
                if (Convert.ToDouble(miDataTable.Rows[0][9].ToString()) > 0)
                {
                    medioPago = new MedioDePago();
                    medioPago.TipoMedio = MedioDePago.TipoMedioPago.Cheque;
                    medioPago.Banco = miDataTable.Rows[0][6].ToString();
                    medioPago.NumeroChequeTarjeta = miDataTable.Rows[0][7].ToString();
                    medioPago.Cuenta = miDataTable.Rows[0][8].ToString();
                    medioPago.Valor = Convert.ToDouble(miDataTable.Rows[0][9].ToString());
                    unPago.MediosDePago.Add(medioPago);
                }
                //Transferencia
                if (Convert.ToDouble(miDataTable.Rows[0][11].ToString()) > 0)
                {
                    medioPago = new MedioDePago();
                    medioPago.TipoMedio = MedioDePago.TipoMedioPago.Transferencia;
                    medioPago.Cuenta = miDataTable.Rows[0][10].ToString();
                    medioPago.Valor = Convert.ToDouble(miDataTable.Rows[0][11].ToString());
                    unPago.MediosDePago.Add(medioPago);
                }
                Saldo saldo;
                for (int i = 0; i < miDataTable.Rows.Count; i++)
                {
                    saldo = new Saldo();
                    if (miDataTable.Rows[i][12].ToString().Equals("Asiento"))
                        saldo.Tipo = Saldo.TipoDocumento.Asiento;
                    else
                        saldo.Tipo = miDataTable.Rows[i][12].ToString().Equals("S") ? Saldo.TipoDocumento.NotaDebito : Saldo.TipoDocumento.Factura;
                    saldo.DocEntry = Convert.ToInt32(miDataTable.Rows[i][14].ToString());
                    saldo.Total = 0;
                    saldo.Valoraplicar = Convert.ToDouble(miDataTable.Rows[i][15].ToString()); ;
                    saldo.SaldoPendiente = 0;
                    saldo.LineId = Convert.ToInt32(miDataTable.Rows[i][13].ToString());
                    saldo.Seleccionado = true;
                    saldo.Numero = Convert.ToInt32(miDataTable.Rows[i][24].ToString());
                    saldo.Fecha = Convert.ToDateTime(miDataTable.Rows[i][25].ToString());
                    saldo.Total = Convert.ToDouble(miDataTable.Rows[i][26].ToString());
                    saldo.SaldoPendiente = Convert.ToDouble(miDataTable.Rows[i][27].ToString());
                    unPago.Saldos.Add(saldo);
                }
            }
        }
        /// <summary>
        /// Recupera un pago
        /// </summary>        
        public void ObtenerPago(PagoTO unPago)
        {
            DateTime fecha = DateTime.Now;
            List<Cuenta> miListaCuentas = new List<Cuenta>(0);
            StringBuilder Consulta = new StringBuilder();
            InformacionPorPago infoPago = GlobalesPagos.informacionPagos.Find(x => (int)x.TipoDePago == (int)unPago.Tipo);
            Consulta.Append("SELECT T0.DocDate, T0.TaxDate, T0.CardCode, T0.CardName,  ");
            Consulta.Append("T0.CashSum, T0.CashAcct,  ");//--Efectivo
            Consulta.Append("T0.BankCode, T2.CheckNum, T0.CheckAcct, T0.CheckSum,  ");//--Cheque
            Consulta.Append("T0.TrsfrAcct, T0.TrsfrSum,   ");//--Transferencia
            Consulta.Append("T1.InvType, T1.DocLine, T1.DocEntry, T1.SumApplied, T0.DocTotal, T3.U_CSS_Type, T3.Credit, T1.SumApplied, T0.Comments, T0.DocNum, T4.Ref3, isnull (T1.U_CSS_Comision, 0) ");
            Consulta.Append("FROM ORCT T0 ");
            Consulta.Append("INNER JOIN RCT2 T1 ON T0.DocNum=T1.DocNum ");
            Consulta.Append("LEFT JOIN RCT1 T2 ON T2.DocNum=T0.DocNum ");
            Consulta.Append("INNER JOIN JDT1 T3 ON T3.Line_ID=T1.DocLine ");
            Consulta.Append("AND T3.TransId = T1.DocEntry ");
            Consulta.Append("INNER JOIN OJDT T4 ON T3.TransId=T4.TransId ");
            Consulta.Append("WHERE T0.DocType='C' AND T0.Canceled = 'N'");
            if (unPago.Movimiento == PagoTO.EnumMovimiento.Actual)
                Consulta.Append("AND T0.DocNum = @NumeroPago AND T0.Series = @Series ");
            else if (unPago.Movimiento == PagoTO.EnumMovimiento.Primero)
                Consulta.Append("AND T0.DocNum = (SELECT MIN(DocNum) FROM ORCT WHERE DocType='C' AND T0.Series = @Series) ");
            else if (unPago.Movimiento == PagoTO.EnumMovimiento.Anterior)
                Consulta.Append("AND T0.DocNum = (SELECT TOP 1 DocNum FROM ORCT WHERE DocType='C'  AND T0.Series = @Series AND DocNum<@NumeroPago ORDER BY 1 DESC)");
            else if (unPago.Movimiento == PagoTO.EnumMovimiento.Siguiente)
                Consulta.Append("AND T0.DocNum = (SELECT TOP 1 DocNum FROM ORCT WHERE DocType='C'  AND T0.Series = @Series AND DocNum>@NumeroPago ORDER BY 1 ASC)");
            else if (unPago.Movimiento == PagoTO.EnumMovimiento.Ultimo)
                Consulta.Append("AND T0.DocNum = (SELECT MAX(DocNum) FROM ORCT WHERE DocType='C'  AND T0.Series = @Series) ");
            DataSet miDataSet = new DataSet();
            DbCommand miComando = this.conexionBD.baseDatos.GetSqlStringCommand(Consulta.ToString());
            this.conexionBD.baseDatos.AddInParameter(miComando, "@NumeroPago", DbType.String, unPago.DocNum);
            this.conexionBD.baseDatos.AddInParameter(miComando, "@Series", DbType.String, infoPago.Serie);
            miDataSet = this.conexionBD.baseDatos.ExecuteDataSet(miComando);
            DataTable miDataTable = new DataTable();
            miDataTable = miDataSet.Tables[0];
            if (miDataTable.Rows.Count > 0)
            {
                //Encabezado
                unPago.Fecha = Convert.ToDateTime(miDataTable.Rows[0][0]);
                unPago.Socio.CardCode  = miDataTable.Rows[0][2].ToString();
                unPago.Socio.CardName  = miDataTable.Rows[0][3].ToString();
                unPago.Total = Convert.ToDouble(miDataTable.Rows[0][16].ToString());
                unPago.Comentarios = miDataTable.Rows[0][20].ToString();
                unPago.DocNum = Convert.ToInt32(miDataTable.Rows[0][21].ToString());
                //Medios de pago
                //Efectivo
                MedioDePago medioPago = new MedioDePago();
                if (Convert.ToDouble(miDataTable.Rows[0][4].ToString()) > 0)
                {
                    medioPago.TipoMedio = MedioDePago.TipoMedioPago.Efectivo;
                    medioPago.Valor = Convert.ToDouble(miDataTable.Rows[0][4].ToString());
                    medioPago.Cuenta = miDataTable.Rows[0][5].ToString();
                    unPago.MediosDePago.Add(medioPago);
                }
                //Cheque
                if (Convert.ToDouble(miDataTable.Rows[0][9].ToString()) > 0)
                {
                    medioPago = new MedioDePago();
                    medioPago.TipoMedio = MedioDePago.TipoMedioPago.Cheque;
                    medioPago.Banco = miDataTable.Rows[0][6].ToString();
                    medioPago.NumeroChequeTarjeta = miDataTable.Rows[0][7].ToString();
                    medioPago.Cuenta = miDataTable.Rows[0][8].ToString();
                    medioPago.Valor = Convert.ToDouble(miDataTable.Rows[0][9].ToString());
                    unPago.MediosDePago.Add(medioPago);
                }
                //Transferencia
                if (Convert.ToDouble(miDataTable.Rows[0][11].ToString()) > 0)
                {
                    medioPago = new MedioDePago();
                    medioPago.TipoMedio = MedioDePago.TipoMedioPago.Transferencia;
                    medioPago.Cuenta = miDataTable.Rows[0][10].ToString();
                    medioPago.Valor = Convert.ToDouble(miDataTable.Rows[0][11].ToString());
                    unPago.MediosDePago.Add(medioPago);
                }
                RemisionCobro remision;
                RemisionCobroDetalle remisionDetalle = new RemisionCobroDetalle();
                //foreach (DataRow miFila in miDataTable.Rows)
                remision = new RemisionCobro();
                for (int i = 0; i <= miDataTable.Rows.Count; i++)
                {
                    if (i > 0)
                        if (i == miDataTable.Rows.Count)
                        {
                            remision.NumeroRemisionCobro = miDataTable.Rows[i - 1][22].ToString();
                            remision.Seleccionado = true;
                            remision.ConComision = true;
                            remision.Comision = Convert.ToDouble(miDataTable.Rows[i - 1][23]);
                            unPago.RemisionesCobro.Add(remision);
                            return;
                        }
                        else if (!miDataTable.Rows[i][22].Equals(miDataTable.Rows[i - 1][22]))
                        {
                            remision.NumeroRemisionCobro = miDataTable.Rows[i - 1][22].ToString();
                            remision.Seleccionado = true;
                            remision.ConComision = true;
                            remision.Comision = Convert.ToDouble(miDataTable.Rows[i - 1][23]);
                            unPago.RemisionesCobro.Add(remision);
                            remision = new RemisionCobro();
                        }
                    remisionDetalle.Crebito = Convert.ToDouble(miDataTable.Rows[i][18]);
                    remisionDetalle.Debito = Convert.ToDouble(miDataTable.Rows[i][19]);

                    //remision.TransId = Convert.ToInt32(miDataTable.Rows[i][14]);
                    //remision.Prima = Convert.ToDouble(miDataTable.Rows[i][15]);

                    switch (miDataTable.Rows[i][17].ToString())
                    {

                        case "Vlr. Com. Propia": remisionDetalle.TipoRegistro = RemisionCobroDetalle.Tipo.Vlr_Com_Propia;
                            break;
                        case "Vlr. Gastos": remisionDetalle.TipoRegistro = RemisionCobroDetalle.Tipo.Vlr_Gastos;
                            if (remisionDetalle.Crebito > 0)
                                remision.TotalPolizaCreditos += remisionDetalle.Crebito;
                            else
                                remision.TotalPoliza += remisionDetalle.Debito;
                            //remision.TotalPolizasDebitos += remisionDetalle.Debito;
                            remision.Gastos = Convert.ToDouble(miDataTable.Rows[i][15]);
                            break;
                        case "Vlr. ICA sobre Com.": remisionDetalle.TipoRegistro = RemisionCobroDetalle.Tipo.Vlr_ICA_sobre_Com;
                            break;
                        case "Vlr. IVA": remisionDetalle.TipoRegistro = RemisionCobroDetalle.Tipo.VlrIVA;
                            if (remisionDetalle.Crebito > 0)
                                remision.TotalPolizaCreditos += remisionDetalle.Crebito;
                            else
                                remision.TotalPoliza += remisionDetalle.Debito;
                            //remision.TotalPolizasDebitos += remisionDetalle.Debito;
                            remision.IVA = Convert.ToDouble(miDataTable.Rows[i][15]);
                            break;
                        case "Vlr. Prima": remisionDetalle.TipoRegistro = RemisionCobroDetalle.Tipo.VlrPrima;
                            if (remisionDetalle.Crebito > 0)
                                remision.TotalPolizaCreditos += remisionDetalle.Crebito;
                            else
                                remision.TotalPoliza += remisionDetalle.Debito;
                            //remision.TotalPolizasDebitos += remisionDetalle.Debito;
                            remision.Prima = Convert.ToDouble(miDataTable.Rows[i][15]);
                            break;
                        case "x Vlr. IVA sobre Com.": remisionDetalle.TipoRegistro = RemisionCobroDetalle.Tipo.xVlrIVAsobreCom;
                            break;
                    }
                }
            }
        }

       



       


        /// <summary>
        /// Recupera el Listado de forma de pago de SAP
        /// </summary>
        /// <returns>Listado de formas de pago</returns>
        //////public DataTable ObtenerFormasPagos()
        //////{
        //////    StringBuilder Consulta = new StringBuilder();
        //////    Consulta.Append("SELECT CrTypeCode, CrTypeName FROM OCRP");
        //////    DataSet miDataSet = new DataSet();
        //////    DbCommand miComando = this.conexionBD.baseDatos.GetSqlStringCommand(Consulta.ToString());
        //////    miDataSet = this.conexionBD.baseDatos.ExecuteDataSet(miComando);
        //////    DataTable miDataTable = new DataTable();
        //////    miDataTable = miDataSet.Tables[0];
        //////    return miDataTable;
        //////}

       
    }
}
