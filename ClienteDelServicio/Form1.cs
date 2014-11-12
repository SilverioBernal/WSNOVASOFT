using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClienteDelServicio.Contable_srv;
using System.ServiceModel;



namespace ClienteDelServicio
{
    public partial class Form1 : Form
    {
        public Form1()
        {
           // InitializeComponent();
        }

        private void CmdPagos_Click(object sender, EventArgs e)
        {
            int respuesta=0;
            //Contable_srv.PagoTO   oDatosDelPago = new Contable_srv.PagoTO();
            Contable_srv.WsNsftContabilidadClient oPago = new Contable_srv.WsNsftContabilidadClient();

            Asiento asiento = new Asiento()
            {
                codigoTransaccion = "PAGOS",
            };


            AsientoDetalle linea1 = new AsientoDetalle() {
                AccountCode = "91050103",
                 Credit = 1000
            };
            AsientoDetalle linea2 = new AsientoDetalle()
            {
                AccountCode = "19202010",
                Debit  = 1000
            };

            asiento.lineas.Add(linea1);
            asiento.lineas.Add(linea2);

            oPago.CrearAsientoContable(asiento, new ConexionWS() { Usuario = "Paquito", Contrasena = "Gallego" });

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int respuesta = 0;
            //Contable_srv.PagoTO   oDatosDelPago = new Contable_srv.PagoTO();
            Contable_srv.WsNsftContabilidadClient oPago = new Contable_srv.WsNsftContabilidadClient();

            Asiento asiento = new Asiento()
            {
                codigoTransaccion = "PAGOS",
            };


            AsientoDetalle linea1 = new AsientoDetalle()
            {
                AccountCode = "91050103",
                Credit = 1000
            };
            AsientoDetalle linea2 = new AsientoDetalle()
            {
                AccountCode = "19202010",
                Debit = 1000
            };

            asiento.lineas.Add(linea1);
            asiento.lineas.Add(linea2);

            oPago.CrearAsientoContable(asiento, new ConexionWS() { Usuario = "Paquito", Contrasena = "Gallego" });

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int respuesta = 0;
            //Contable_srv.PagoTO   oDatosDelPago = new Contable_srv.PagoTO();
            Contable_srv.WsNsftContabilidadClient oPago = new Contable_srv.WsNsftContabilidadClient();

            Asiento asiento = new Asiento()
            {
                codigoTransaccion = "PAGOS",
            };


            AsientoDetalle linea1 = new AsientoDetalle()
            {
                AccountCode = "91050103",
                Credit = 1000
            };
            AsientoDetalle linea2 = new AsientoDetalle()
            {
                AccountCode = "19202010",
                Debit = 1000
            };

            asiento.lineas.Add(linea1);
            asiento.lineas.Add(linea2);

            oPago.CrearAsientoContable(asiento, new ConexionWS() { Usuario = "Paquito", Contrasena = "Gallego" });

        }
    }
}
