using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCPConnectivity;
using TCPConnectivity.EventArgs;

namespace ServerSide
{
    public partial class B : Form
    {
        private static List<object> _list;
        private static Server _server;
        public B()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            _list = new List<object>();
            _server = new Server("127.0.1", 42422);
            _server.Receive += Server_Receive;
        }

        private void Server_Receive(object sender, SocketArgs e)
        {
            TransferArgs args = e as TransferArgs;
            string data = Encoding.ASCII.GetString(args.Get());
            dynamic message = Newtonsoft.Json.JsonConvert.DeserializeObject(data);
            _list.Add(new
            {
                Veri1 = message.StringData,
                Veri2 = message.IntData,
                Veri3 = message.FloatData,
                ReceiveDate = DateTime.Now.ToString(),
                Id = _server.ConnectionIPs[((IPEndPoint)args.Connection.LocalEndPoint).Address.ToString()]
            });
            lock (this)
            {
                grdB.DataSource = null;
                grdB.DataSource = _list;
            }
        }
    }
}
