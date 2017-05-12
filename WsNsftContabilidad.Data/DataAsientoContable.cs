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

        private IDataReader reader;
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

            #region Encabezado
            miAsientoContable = (JournalEntries)ConexionSAP.Conexion.compania.GetBusinessObject(BoObjectTypes.oJournalEntries);

            if (!string.IsNullOrEmpty(unAsiento.Memo))
                if (unAsiento.Memo.ToString().Length > 50)
                    miAsientoContable.Memo = unAsiento.Memo.ToString().Substring(0, 49);
                else
                    miAsientoContable.Memo = unAsiento.Memo.ToString();

            if (!string.IsNullOrEmpty(unAsiento.Ref1))
                if (unAsiento.Ref1.ToString().Length > 100)
                    miAsientoContable.Reference = unAsiento.Ref1.ToString().Substring(0, 99);
                else
                    miAsientoContable.Reference = unAsiento.Ref1.ToString();

            if (!string.IsNullOrEmpty(unAsiento.Ref2))
                if (unAsiento.Ref2.ToString().Length > 100)
                    miAsientoContable.Reference2 = unAsiento.Ref2.ToString().Substring(0, 99);
                else
                    miAsientoContable.Reference2 = unAsiento.Ref2.ToString();

            if (!string.IsNullOrEmpty(unAsiento.TransCode))
                if (unAsiento.TransCode.ToString().Length > 4)
                    miAsientoContable.TransactionCode = unAsiento.TransCode.ToString().Substring(0, 3);
                else
                    miAsientoContable.TransactionCode = unAsiento.TransCode.ToString();

            if (!string.IsNullOrEmpty(unAsiento.Project))
                if (unAsiento.Project.ToString().Length > 8)
                    miAsientoContable.ProjectCode = unAsiento.Project.ToString().Substring(0, 7);
                else
                    miAsientoContable.ProjectCode = unAsiento.Project.ToString();

            if (unAsiento.TaxDate != null)
                miAsientoContable.TaxDate = (DateTime)unAsiento.TaxDate;

            if (!string.IsNullOrEmpty(unAsiento.indicator))
                if (unAsiento.indicator.ToString().Length > 2)
                    miAsientoContable.Indicator = unAsiento.indicator.ToString().Substring(0, 1);
                else
                    miAsientoContable.Indicator = unAsiento.indicator.ToString();

            if (unAsiento.AutoStorno != null)
                if (unAsiento.AutoStorno == true)
                    miAsientoContable.UseAutoStorno = BoYesNoEnum.tYES;
                else
                    miAsientoContable.UseAutoStorno = BoYesNoEnum.tNO;

            if (unAsiento.StornoDate != null)
                miAsientoContable.StornoDate = (DateTime)unAsiento.StornoDate;

            if (unAsiento.VatDate != null)
                miAsientoContable.VatDate = (DateTime)unAsiento.VatDate;

            if (unAsiento.series != null)
                miAsientoContable.Series = (int) unAsiento.series;

            if (unAsiento.StampTax != null)
                if (unAsiento.StampTax == true)
                    miAsientoContable.StampTax = BoYesNoEnum.tYES;
                else
                    miAsientoContable.StampTax = BoYesNoEnum.tNO;

            if (unAsiento.AutoVat != null)
                if (unAsiento.AutoVat == true)
                    miAsientoContable.AutoVAT = BoYesNoEnum.tYES;
                else
                    miAsientoContable.AutoVAT = BoYesNoEnum.tNO;

            if (unAsiento.ReportEU != null)
                if (unAsiento.ReportEU == true)
                    miAsientoContable.ReportEU = BoYesNoEnum.tYES;
                else
                    miAsientoContable.ReportEU = BoYesNoEnum.tNO;

            if (unAsiento.Report347 != null)
                if (unAsiento.Report347 == true)
                    miAsientoContable.Report347 = BoYesNoEnum.tYES;
                else
                    miAsientoContable.Report347 = BoYesNoEnum.tNO;

            if (unAsiento.Location != null)
                miAsientoContable.LocationCode = (int)unAsiento.Location;

            if (unAsiento.BlockDunn != null)
                if (unAsiento.BlockDunn == true)
                    miAsientoContable.BlockDunningLetter = BoYesNoEnum.tYES;
                else
                    miAsientoContable.BlockDunningLetter = BoYesNoEnum.tNO;

            if (unAsiento.AutoWT != null)
                if (unAsiento.AutoWT == true)
                    miAsientoContable.AutomaticWT = BoYesNoEnum.tYES;
                else
                    miAsientoContable.AutomaticWT = BoYesNoEnum.tNO;

            if (unAsiento.Corisptivi != null)
                if (unAsiento.Corisptivi == true)
                    miAsientoContable.Corisptivi = BoYesNoEnum.tYES;
                else
                    miAsientoContable.Corisptivi = BoYesNoEnum.tNO;
            #endregion

            // Adicion de detalle de asiento 

            #region Detalle
            foreach (AsientoDetalle linea in unAsiento.lineas)
            {
                #region Manejo de cuentas asociadas y/o tercero
                if (CuentaAsociada(linea.Account))
                {
                    if (!string.IsNullOrEmpty(linea.socioNegocio.CardCode))
                    {
                        miAsientoContable.Lines.ShortName = linea.socioNegocio.CardCode;
                        miAsientoContable.Lines.UserFields.Fields.Item("U_InfoCo01").Value = linea.socioNegocio.CardCode;
                    }
                    else
                    {
                        miAsientoContable.Lines.ShortName = linea.U_InfoCo01 == null ? "" : linea.U_InfoCo01;
                        miAsientoContable.Lines.UserFields.Fields.Item("U_InfoCo01").Value = linea.U_InfoCo01 == null ? "" : linea.U_InfoCo01; ;
                    }
                }
                else
                {
                    miAsientoContable.Lines.AccountCode = linea.Account;

                    if (CuentaReqTercero(linea.Account))
                    {
                        if (!string.IsNullOrEmpty(linea.socioNegocio.CardCode))
                            miAsientoContable.Lines.UserFields.Fields.Item("U_InfoCo01").Value = linea.socioNegocio.CardCode;
                        else
                            miAsientoContable.Lines.UserFields.Fields.Item("U_InfoCo01").Value = linea.U_InfoCo01 == null ? "" : linea.U_InfoCo01;
                    }
                } 
                #endregion

                miAsientoContable.Lines.Debit = linea.Debit;
                miAsientoContable.Lines.Credit = linea.Credit;

                if (linea.FCDebit != null)
                    miAsientoContable.Lines.FCDebit = (double)linea.FCDebit;

                if (linea.FCCredit != null)
                    miAsientoContable.Lines.FCCredit = (double)linea.FCCredit;

                if (!string.IsNullOrEmpty(linea.FCCurrency))
                    if (linea.FCCurrency.ToString().Length > 3)
                        miAsientoContable.Lines.FCCurrency = linea.FCCurrency.ToString().Substring(0, 2);
                    else
                        miAsientoContable.Lines.FCCurrency = linea.FCCurrency.ToString();

                if(linea.DuoDate != null)
                    miAsientoContable.Lines.DueDate = (DateTime)linea.DuoDate;

                if (!string.IsNullOrEmpty(linea.ContraAct))
                    if (linea.ContraAct.ToString().Length > 15)
                        miAsientoContable.Lines.FCCurrency = linea.ContraAct.ToString().Substring(0, 14);
                    else
                        miAsientoContable.Lines.FCCurrency = linea.ContraAct.ToString();

                if (!string.IsNullOrEmpty(linea.LineMemo))
                    if (linea.LineMemo.ToString().Length > 50)
                        miAsientoContable.Lines.LineMemo = linea.LineMemo.ToString().Substring(0, 49);
                    else
                        miAsientoContable.Lines.LineMemo = linea.LineMemo.ToString();

                if (linea.RefDate != null)
                    miAsientoContable.Lines.ReferenceDate1 = (DateTime)linea.RefDate;

                if (linea.Ref2Date != null)
                    miAsientoContable.Lines.ReferenceDate2 = (DateTime)linea.Ref2Date;

                if (!string.IsNullOrEmpty(linea.Ref1))
                    if (linea.Ref1.ToString().Length > 100)
                        miAsientoContable.Lines.Reference1 = linea.Ref1.ToString().Substring(0, 99);
                    else
                        miAsientoContable.Lines.Reference1 = linea.Ref1.ToString();

                if (!string.IsNullOrEmpty(linea.Ref2))
                    if (linea.Ref2.ToString().Length > 100)
                        miAsientoContable.Lines.Reference2 = linea.Ref2.ToString().Substring(0, 99);
                    else
                        miAsientoContable.Lines.Reference2 = linea.Ref2.ToString();

                if (!string.IsNullOrEmpty(linea.Project))
                    if (linea.Project.ToString().Length > 8)
                        miAsientoContable.Lines.ProjectCode = linea.Project.ToString().Substring(0, 7);
                    else
                        miAsientoContable.Lines.ProjectCode = linea.Project.ToString();

                if (!string.IsNullOrEmpty(linea.ProfitCode))
                    if (linea.ProfitCode.ToString().Length > 8)
                        miAsientoContable.Lines.CostingCode = linea.ProfitCode.ToString().Substring(0, 7);
                    else
                        miAsientoContable.Lines.CostingCode = linea.ProfitCode.ToString();

                if (linea.TaxDate != null)
                    miAsientoContable.Lines.TaxDate = (DateTime)linea.TaxDate;

                if (linea.BaseSum != null)
                    miAsientoContable.Lines.BaseSum = (double)linea.BaseSum;

                if (!string.IsNullOrEmpty(linea.VatGroup))
                    if (linea.VatGroup.ToString().Length > 8)
                        miAsientoContable.Lines.TaxGroup = linea.VatGroup.ToString().Substring(0, 7);
                    else
                        miAsientoContable.Lines.TaxGroup = linea.VatGroup.ToString();

                if (linea.SYSDeb != null)
                    miAsientoContable.Lines.DebitSys = (double)linea.SYSDeb;

                if (linea.SYSCred != null)
                    miAsientoContable.Lines.CreditSys = (double)linea.SYSCred;

                if (linea.VatDate != null)
                    miAsientoContable.Lines.VatDate = (DateTime)linea.VatDate;

                if (linea.VatLine != null)
                    if (linea.VatLine == true)
                        miAsientoContable.Lines.VatLine = BoYesNoEnum.tYES;
                    else
                        miAsientoContable.Lines.VatLine = BoYesNoEnum.tNO;

                if (linea.SYSBaseSum != null)
                    miAsientoContable.Lines.SystemBaseAmount = (double)linea.SYSBaseSum;

                if (linea.VatAmount != null)
                    miAsientoContable.Lines.VatAmount = (double)linea.VatAmount;

                if (linea.SYSVatSum != null)
                    miAsientoContable.Lines.SystemVatAmount = (double)linea.SYSVatSum;

                if (linea.GrossValue != null)
                    miAsientoContable.Lines.GrossValue = (double)linea.GrossValue;

                if (!string.IsNullOrEmpty(linea.Ref3Line))
                    if (linea.Ref3Line.ToString().Length > 27)
                        miAsientoContable.Lines.AdditionalReference = linea.Ref3Line.ToString().Substring(0, 26);
                    else
                        miAsientoContable.Lines.AdditionalReference = linea.Ref3Line.ToString();

                if (!string.IsNullOrEmpty(linea.OcrCode2))
                    if (linea.OcrCode2.ToString().Length > 8)
                        miAsientoContable.Lines.CostingCode2 = linea.OcrCode2.ToString().Substring(0, 7);
                    else
                        miAsientoContable.Lines.CostingCode2 = linea.OcrCode2.ToString();

                if (!string.IsNullOrEmpty(linea.OcrCode3))
                    if (linea.OcrCode3.ToString().Length > 8)
                        miAsientoContable.Lines.CostingCode3 = linea.OcrCode3.ToString().Substring(0, 7);
                    else
                        miAsientoContable.Lines.CostingCode3 = linea.OcrCode3.ToString();

                if (!string.IsNullOrEmpty(linea.OcrCode4))
                    if (linea.OcrCode4.ToString().Length > 8)
                        miAsientoContable.Lines.CostingCode4 = linea.OcrCode4.ToString().Substring(0, 7);
                    else
                        miAsientoContable.Lines.CostingCode4 = linea.OcrCode4.ToString();

                if (!string.IsNullOrEmpty(linea.TaxCode))
                    if (linea.TaxCode.ToString().Length > 8)
                        miAsientoContable.Lines.TaxCode = linea.TaxCode.ToString().Substring(0, 7);
                    else
                        miAsientoContable.Lines.TaxCode = linea.TaxCode.ToString();

                if (linea.TaxPostAccount !=null)
                    switch (linea.TaxPostAccount)
                    {
                        case TaxPostingAccount.tpa_Default:
                            miAsientoContable.Lines.TaxPostAccount = BoTaxPostAccEnum.tpa_Default;
                            break;
                        case TaxPostingAccount.tpa_SalesTaxAccount:
                            miAsientoContable.Lines.TaxPostAccount = BoTaxPostAccEnum.tpa_SalesTaxAccount;
                            break;
                        case TaxPostingAccount.tpa_PurchaseTaxAccount:
                            miAsientoContable.Lines.TaxPostAccount = BoTaxPostAccEnum.tpa_PurchaseTaxAccount;
                            break;
                        default:
                            break;
                    }

                if (!string.IsNullOrEmpty(linea.OcrCode5))
                    if (linea.OcrCode5.ToString().Length > 8)
                        miAsientoContable.Lines.CostingCode5 = linea.OcrCode5.ToString().Substring(0, 7);
                    else
                        miAsientoContable.Lines.CostingCode5 = linea.OcrCode5.ToString();

                if (linea.Location != null)
                    miAsientoContable.Lines.LocationCode = (int)linea.Location;

                if (!string.IsNullOrEmpty(linea.ControlAccount))
                    if (linea.ControlAccount.ToString().Length > 15)
                        miAsientoContable.Lines.ControlAccount = linea.ControlAccount.ToString().Substring(0, 14);
                    else
                        miAsientoContable.Lines.ControlAccount = linea.ControlAccount.ToString();

                if (linea.WTLiable != null)
                    if (linea.WTLiable == true)
                        miAsientoContable.Lines.WTLiable = BoYesNoEnum.tYES;
                    else
                        miAsientoContable.Lines.WTLiable = BoYesNoEnum.tNO;

                if (linea.WTLine != null)
                    if (linea.WTLine == true)
                        miAsientoContable.Lines.WTRow = BoYesNoEnum.tYES;
                    else
                        miAsientoContable.Lines.WTRow = BoYesNoEnum.tNO;

                if (linea.PayBlock != null)
                    if (linea.PayBlock == true)
                        miAsientoContable.Lines.PaymentBlock = BoYesNoEnum.tYES;
                    else
                        miAsientoContable.Lines.PaymentBlock = BoYesNoEnum.tNO;

                if (linea.PayBlckRef != null)
                    miAsientoContable.Lines.BlockReason = (int)linea.PayBlckRef;

                miAsientoContable.Lines.Add();
            }

            #endregion

            //Creacion del asiento contable
            miResultado = miAsientoContable.Add();
            if (miResultado != 0)
                throw new SAPException(miResultado, ConexionSAP.Conexion.compania.GetLastErrorDescription());
            else
                return Convert.ToInt32(ConexionSAP.Conexion.compania.GetNewObjectKey());
        }

        public bool CuentaReqTercero(string codigoCuenta)
        {
            StringBuilder miSentencia = new StringBuilder("select u_infoco02 from OACT where AcctCode = @acctCode");

            DbCommand miComando = this.baseDatos.GetSqlStringCommand(miSentencia.ToString());
            this.baseDatos.AddInParameter(miComando, "acctCode", DbType.String, codigoCuenta);


            using (this.reader = this.baseDatos.ExecuteReader(miComando))
            {
                while (this.reader.Read())
                {
                    if (this.reader.GetValue(0).ToString() == "1")
                        return true;
                }
            }
            return false;
        }

        public bool CuentaAsociada(string codigoCuenta)
        {
            StringBuilder miSentencia = new StringBuilder("select LocManTran from OACT where AcctCode = @acctCode");

            DbCommand miComando = this.baseDatos.GetSqlStringCommand(miSentencia.ToString());
            this.baseDatos.AddInParameter(miComando, "acctCode", DbType.String, codigoCuenta);


            using (this.reader = this.baseDatos.ExecuteReader(miComando))
            {
                while (this.reader.Read())
                {
                    if (this.reader.GetValue(0).ToString() == "Y")
                        return true;
                }
            }
            return false;
        }
        #endregion
    }
}
