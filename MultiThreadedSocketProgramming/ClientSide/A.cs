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

namespace ClientSide
{
    public partial class A : Form
    {
        private static Client _client;
        public A()
        {
            InitializeComponent();
            _client = new Client("127.0.0.1", 42422);
            _client.Receive += Client_Receive;
            _client.Connect += Client_Connect;

            while (true) { }
        }

        private void Client_Connect(object sender, SocketArgs e)
        {
            var data = Encoding.ASCII.GetBytes("Merhaba server ben client");
            _client.BeginSend(data);
            MessageBox.Show("client gönderme başladı");
        }

        private void Client_Receive(object sender, SocketArgs e)
        {
            TransferArgs args = e as TransferArgs;
            string data = Encoding.ASCII.GetString(args.Get());
            MessageBox.Show(string.Format("data geldi {0}", data));
        }
    }
}
