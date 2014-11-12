using ClienteDelServicio.Contable_srv;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClienteDelServicio
{
    public partial class Form2 : Form
    {
        public Form2()
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
                Account = "91050103",
                Credit = 1000
            };
            AsientoDetalle linea2 = new AsientoDetalle()
            {
                Account = "19202010",
                Debit = 1000
            };

            asiento.lineas.Add(linea1);
            asiento.lineas.Add(linea2);

            int asientoNum = oPago.CrearAsientoContable(asiento, new ConexionWS() { Usuario = "Paquito", Contrasena = "Gallego" });
            MessageBox.Show(asientoNum.ToString());
        }
    }
}
