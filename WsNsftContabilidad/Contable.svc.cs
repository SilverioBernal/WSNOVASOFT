using System;
using System.ServiceModel;
using WsNsftContabilidad.Business;
using WsNsftContabilidad.Business.Entities.Asientos;
using WsNsftContabilidad.Business.Entities.Pagos;
using WsNsftContabilidad.Business.Entities.Seguridad;
using WsNsftContabilidad.Business.Entities.SociosNegocio;


namespace WsNsftContabilidad
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class Contable : IContable
    {
        #region Atributos
        /// <summary>
        /// Acceso a la capa de negocios
        /// </summary>
        private BusinessFachade fachada;
        #endregion

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
    }
}
