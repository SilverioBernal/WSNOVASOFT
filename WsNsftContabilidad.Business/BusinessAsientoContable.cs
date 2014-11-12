﻿using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Runtime.InteropServices;
using WsNsftContabilidad.Business.Entities;
using WsNsftContabilidad.Business.Entities.Asientos;
using WsNsftContabilidad.Business.Entities.Seguridad;
using WsNsftContabilidad.Business.Entities.SociosNegocio;
using WsNsftContabilidad.Data;
//using WsNsftContabilidad.Data.Pagos;


namespace WsNsftContabilidad.Business
{
    public class BusinessAsientoContable
    {
        #region Atributos
        // ES ESTATICO NO REQUIERE Util utilidades;
        //ConexionSAP sapData;
        private DataConexionSAP sapData;
        private BusinessSocioNegocio socioData;
        private DataAsientoContable asientosData;
        #endregion

        #region Constructor
        public BusinessAsientoContable()
        {
            asientosData = new DataAsientoContable();
            //no requiere implementación utilidades = new Util();
            sapData = new DataConexionSAP();
            //no requiere implementación socioData = new SociosSAP();
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Generar el pago en SAP
        /// </summary>
        /// <returns>Listado de Cuentas</returns>
        public int CrearAsiento(Asiento asientoContable, ConexionWS conexion)
        {
            //if (!Util.ValidarDatosAccesoServicio(conexion))
            //    throw new BusinessException(15, "Nombre de Usuario o Contraseña incorrecta para el Servicio");
            int numeroAsiento = -1;
            if (sapData.Conectar())
            {
                #region Contenido del Asiento
                try
                {
                    sapData.IniciarTransaccion();

                    if (asientoContable.lineas.Count > 0)
                    {
                        foreach (AsientoDetalle item in asientoContable.lineas)
                        {
                            if (item.socioNegocio != null)
                            {
                                SocioNegocio socio = socioData.ConsultarSocio(item.socioNegocio.CardCode, conexion);

                                if (socio == null)
                                    socioData.CrearSocio(item.socioNegocio, conexion);
                            }
                        }

                        numeroAsiento = asientosData.CrearAsiento(asientoContable);
                    }
                    sapData.TerminarTransaccion(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                    return numeroAsiento;
                }
                catch (SAPException ex)
                {
                    sapData.TerminarTransaccion(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    Util.ProcesarSapException(ex, "Gestión de Pagos");
                    return numeroAsiento;
                }
                catch (COMException ex)
                {
                    sapData.TerminarTransaccion(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    Exception outEx;
                    if (ExceptionPolicy.HandleException(ex, "Politica_Excepcion_Com", out outEx))
                    {
                        outEx.Data.Add("1", "3");
                        outEx.Data.Add("2", "NA");
                        outEx.Data.Add("3", outEx.Message);
                        throw outEx;
                    }
                    else
                    {
                        throw;
                    }
                    return numeroAsiento;
                }
                catch (Exception ex)
                {
                    sapData.TerminarTransaccion(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    Exception outEx;
                    if (ExceptionPolicy.HandleException(ex, "Politica_ExcepcionGenerica", out outEx))
                    {
                        outEx.Data.Add("1", "3");
                        outEx.Data.Add("2", "NA");
                        outEx.Data.Add("3", outEx.Message);
                        throw outEx;

                    }
                    else
                    {
                        throw ex;
                        return 0;
                    }
                    return numeroAsiento;
                }
                #endregion
                return numeroAsiento;
            }
            return numeroAsiento;
        }
        #endregion
    }
}
