using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientSide.Entity;
using Newtonsoft.Json.Linq;
using TCPConnectivity;
using TCPConnectivity.EventArgs;

namespace ClientSide
{
    public partial class A : Form
    {
        private static Client _client;
        private static List<object> _sendData;
        public A()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            _sendData = new List<object>();
            _client = new Client("127.0.0.1", 42422);
            _client.Connect += Client_Connect;
            _client.Send += Client_Send;
            grdA.AutoGenerateColumns = true;
        }

        private void Client_Send(object sender, SocketArgs e)
        {
            var bytes = (e as TransferArgs).Get();
            string data = Encoding.ASCII.GetString(bytes);
            var message = Newtonsoft.Json.JsonConvert.DeserializeObject<Entity.Message>(data);

            _sendData.Add(new
            {
                Veri1 = message.StringData,
                Veri2 = message.IntData,
                Veri3 = message.FloatData,
                SendData = DateTime.Now.ToString()
            });
            grdA.DataSource = null;
            grdA.DataSource = _sendData;
        }

        private void Client_Connect(object sender, SocketArgs e)
        {
            while (true)
            {
                var msg = new Entity.Message();
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(msg);
                _client.BeginSend(Encoding.ASCII.GetBytes(data));
                System.Threading.Thread.Sleep(2000);
            }
        }

        //private void Client_Receive(object sender, SocketArgs e)
        //{
        //    TransferArgs args = e as TransferArgs;
        //    string data = Encoding.ASCII.GetString(args.Get());
        //    MessageBox.Show(string.Format("data geldi {0}", data));
        //}
    }
}
