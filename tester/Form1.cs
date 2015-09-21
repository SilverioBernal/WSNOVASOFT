using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tester.Contable_srv;

namespace tester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int respuesta = 0;
            //Contable_srv.PagoTO   oDatosDelPago = new Contable_srv.PagoTO();
            Contable_srv.WsNsftContabilidadClient oPago = new Contable_srv.WsNsftContabilidadClient();

            Asiento asiento = new Asiento()
            {
                TransCode = "PAGO",
                Memo = "Hola",
                lineas = new List<AsientoDetalle>()
            };


            AsientoDetalle linea1 = new AsientoDetalle()
            {
                Account = "11150512",
                Credit = 1000
            };
            AsientoDetalle linea2 = new AsientoDetalle()
            {
                Account = "92050103",
                Debit = 1000,
            };


            SocioNegocio bp = new SocioNegocio()
            {
                CardCode = "53054396",
                CardName = "Tatiana Morales",
                LicTradNum = "53054396",
                Cellular = "3165234756",
                Address="CRA 1 No.23-45",
                DebPayAcct = "92050103",
                CustomerCardType = CardType.Cliente
            };
            linea1.socioNegocio = bp;
            linea2.socioNegocio = bp;

            asiento.lineas.Add(linea1);
            asiento.lineas.Add(linea2);

            try
            {
                int asientoNum = oPago.CrearAsientoContable(asiento, new ConexionWS() { Usuario = "Paquito", Contrasena = "Gallego" });
                MessageBox.Show(asientoNum.ToString());
            }
            catch (FaultException<DataAccessFault> ex)
            {
                MessageBox.Show(string.Format("Codigo {0} error:{1} {2}", ex.Code,ex.Detail.Description, ex.Message));
            }            
        }
    }
}
