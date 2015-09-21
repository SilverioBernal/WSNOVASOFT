using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using WsNsftContabilidad.Business;
using WsNsftContabilidad.Business.Entities.Asientos;
using WsNsftContabilidad.Business.Entities.Seguridad;
using WsNsftContabilidad.Services.Contracts;


namespace WsNsftContabilidad.Services.Core
{
    public class WsSAPNovasoft : iWsSAPNovasoft
    {
        #region Atributos
        /// <summary>
        /// Acceso a la capa de negocios
        /// </summary>
        private BusinessFachade fachada;
        #endregion

        #region Métodos
        public int CrearAsientoContable(Asiento asiento, ConexionWS conexion)
        {
            try
            {
                fachada = new BusinessFachade(BusinessFachade.BusinessClass.BusinessAsiento);
                return fachada.CrearAsiento(asiento, conexion);
            }
            catch (Exception ex)
            {
                DataAccessFault detalleError = new DataAccessFault();
                foreach (string valores in ex.Data.Keys)
                {
                    switch (valores)
                    {
                        case "1": detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                        case "2": detalleError.ErrorSAP = ex.Data[valores].ToString();
                            break;
                        case "3": detalleError.Description = ex.Data[valores].ToString();
                            break;
                        default: detalleError.ErrorID = ex.Data[valores].ToString();
                            break;
                    }
                }
                throw new FaultException<DataAccessFault>(detalleError, "Error al Procesar la solicitud");
            }

        } 
        #endregion
    }
}
