using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCPConnectivity;
using TCPConnectivity.EventArgs;

namespace ServerSide
{
    public partial class B : Form
    {
        private static Server _server;
        public B()
        {
            InitializeComponent();
            _server = new Server("127.0.1", 42422);
            _server.Accept += Server_Accept;
            _server.Receive += Server_Receive;
        }

        private void Server_Receive(object sender, SocketArgs e)
        {
            TransferArgs args = e as TransferArgs;
            string data = Encoding.ASCII.GetString(args.Get());
        }

        private void Server_Accept(object sender, SocketArgs e)
        {
            byte[] bytes = Encoding.ASCII.GetBytes("basladi");

            System.Threading.Thread.Sleep(1000);

            _server.BeginSend(e.Connection, bytes);
            _server.BeginSend(e.Connection, bytes);
            _server.BeginSend(e.Connection, bytes);
        }
    }
}
