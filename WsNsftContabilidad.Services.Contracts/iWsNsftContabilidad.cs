//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using System.Runtime.Serialization;
using System.ServiceModel;
using WsNsftContabilidad.Business.Entities.Asientos;
using WsNsftContabilidad.Business.Entities.Seguridad;

namespace WsNsftContabilidad.Services.Contracts
{
    //[ServiceContract]
    [ServiceContract(Namespace = "http://WsNsftContabilidad", Name = "WsNsftContabilidad")]
    public interface iWsNsftContabilidad
    {
        //ASIENTO CONTABLE
        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        int CrearAsientoContable(Asiento asiento, ConexionWS conexion);
    }
}
