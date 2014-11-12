using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsNsftContabilidad.Business.Entities.Pagos
{
    public class InformacionPorPago
    {
        public string Code { get; set; }
        public enum TipoPago
        {
            Normal = 1,
            Directo = 2,
            Innocor = 3,
            Especial = 4,
            LegalizacionesNormal = 5,
            LegalizacionesEspecial = 6,
            NormalEmpresarial = 7,
            NormalTerceros = 8,
            Consignacion = 9
        }
        public TipoPago TipoDePago { get; set; }
        public string Serie { get; set; }
        public List<Impuesto> impuestos  { get; set; }

        public InformacionPorPago()
        {
            impuestos = new List<Impuesto>();
        }
    }
}
