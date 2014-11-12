using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using WsNsftContabilidad.Business.Entities.SociosNegocio;
using System.Data.Common;
using SAPbobsCOM;
using WsNsftContabilidad.Business.Entities.Seguridad;
using WsNsftContabilidad.Business.Entities;

namespace WsNsftContabilidad.Data
{
    /// <summary>
    /// Clase para la gestión de socios de negocios en SAP
    /// </summary>
    public class DataSocio
    {
        #region Atributos
        /// <summary>
        /// Atributos de conexión a la base de datos
        /// </summary>
        private Database baseDatos;
        /// <summary>
        /// Lector
        /// </summary>
        private IDataReader reader;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public DataSocio()
        {
            this.baseDatos = DatabaseFactory.CreateDatabase("SAP");
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Consulta un socio de negocios en SAP Business One
        /// </summary>
        /// <param name="codigo">Codigo de socio de negocio</param>
        /// <returns>Socio con la información</returns>
        public SocioNegocio ConsultarSocio(string codigo)
        {
            StringBuilder miSentencia = new StringBuilder("SELECT  CardCode,CardName, LicTradNum,Phone1, Phone2,Cellular, Fax,E_Mail  ");
            miSentencia.Append("FROM OCRD T0 ");
            miSentencia.Append("WHERE T0.CardCode = @CardCode ");
            miSentencia.Append("AND CardType= 'C' ");
            DbCommand miComando = this.baseDatos.GetSqlStringCommand(miSentencia.ToString());
            this.baseDatos.AddInParameter(miComando, "CardCode", DbType.String, codigo);
            SocioNegocio socio = new SocioNegocio();
            using (this.reader = this.baseDatos.ExecuteReader(miComando))
            {
                while (this.reader.Read())
                {
                    socio = new SocioNegocio();
                    socio.CardCode = this.reader.IsDBNull(0) ? "" : this.reader.GetValue(0).ToString();
                    socio.CardName = this.reader.IsDBNull(1) ? "" : this.reader.GetValue(1).ToString();
                    socio.LicTradNum = this.reader.IsDBNull(2) ? "" : this.reader.GetValue(2).ToString();
                    socio.Phone1 = this.reader.IsDBNull(3) ? "" : this.reader.GetValue(3).ToString();
                    socio.Phone2 = this.reader.IsDBNull(4) ? "" : this.reader.GetValue(4).ToString();
                    socio.Cellular = this.reader.IsDBNull(5) ? "" : this.reader.GetValue(5).ToString();
                    socio.Fax = this.reader.IsDBNull(6) ? "" : this.reader.GetValue(6).ToString();
                    socio.E_Mail = this.reader.IsDBNull(7) ? "" : this.reader.GetValue(7).ToString();
                }
            }
            return socio;
        }

        /// <summary>
        /// Método para la creacion de socios de negocio en SAP
        /// </summary>
        /// <param name="socio"></param>
        public void CrearSocio(SocioNegocio socio)
        {
            BusinessPartners bp; //= new BusinessPartners();

            bp = (BusinessPartners)ConexionSAP.Conexion.compania.GetBusinessObject(BoObjectTypes.oBusinessPartners);

            bp.CardCode = socio.CardCode;
            bp.CardType = BoCardTypes.cCustomer;
            bp.FederalTaxID = socio.CardCode;
            bp.CardName = socio.CardName;

            if (bp.Add() != 0)
                throw new SAPException(ConexionSAP.Conexion.compania.GetLastErrorCode(), ConexionSAP.Conexion.compania.GetLastErrorDescription());

        }
        #endregion
    }
}
