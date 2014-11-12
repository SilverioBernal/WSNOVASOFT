using System.Runtime.Serialization;
using System.ServiceModel;
using WsNsftContabilidad.Business.Entities.Asientos;
using WsNsftContabilidad.Business.Entities.Seguridad;

namespace WsNsftContabilidad
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IContable" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IContable
    {

        //[OperationContract]
        //string GetData(int value);

        //[OperationContract]
        //CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: agregue aquí sus operaciones de servicio
        ////[OperationContract]
        ////public SocioNegocio ConsultarSocio(string codigo, ConexionWS conexion);
        
        /// <summary>
        /// Consulta que permite visualizar un socio en SAP Business One
        /// </summary>
        /// <param name="codigo">Codigo del socio de negocios a consultar.</param>
        /// <param name="conexion">Conexión con el servicio. Para mayor informacion revise la documentacion de entidades</param>
        /// <returns>Información del socio de negocio existente en SAP Business One. Para mayor informacion revise la documentacion de entidades</returns>
        //[OperationContract(IsOneWay = false)]
        //[FaultContract(typeof(DataAccessFault))]
        //SocioNegocio ConsultarSocio(string codigo, ConexionWS conexion);

        //[OperationContract(IsOneWay = false)]
        //[FaultContract(typeof(DataAccessFault))]
        //void CrearSocio(SocioNegocio socio, ConexionWS conexion);

        //pagos
        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(DataAccessFault))]
        int CrearAsientoContable(Asiento asiento, ConexionWS conexion);       
    }


    // Utilice un contrato de datos, como se ilustra en el ejemplo siguiente, para agregar tipos compuestos a las operaciones de servicio.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
