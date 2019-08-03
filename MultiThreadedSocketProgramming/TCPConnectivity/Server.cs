using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TCPConnectivity.EventArgs;

namespace TCPConnectivity
{
    public class Server
    {
        private byte[] _rcvBuffer;
        private static int _id = 0;
        public Server(string host, int port, int maxBufferSize = 1024)
        {
            _rcvBuffer = new byte[maxBufferSize];
            Connections = new List<Socket>();
            ConnectionIPs = new Dictionary<string, int>();
            GeneralBufferSize = maxBufferSize;
            IPEndPoint = new IPEndPoint(IPAddress.Parse(host), port);
            Connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Connection.Bind(IPEndPoint);
            Connection.Listen(int.MaxValue);
            Connection.BeginAccept(AcceptCallback, null);
        }
        public Socket Connection { get; private set; }
        public IPEndPoint IPEndPoint { get; private set; }
        public List<Socket> Connections { get; private set; }
        public Dictionary<string, int> ConnectionIPs { get; set; }
        public int GeneralBufferSize { get; private set; }
        public event ConnectionEventHandler Accept;
        public event ConnectionEventHandler Receive;

        private void AcceptCallback(IAsyncResult ar)
        {
            Socket accepted;
            try
            {
                accepted = Connection.EndAccept(ar);
                Connections.Add(accepted);
                if (!ConnectionIPs.Keys.Contains(((IPEndPoint)accepted.LocalEndPoint).Address.ToString()))
                {
                    ConnectionIPs.Add(((IPEndPoint)accepted.LocalEndPoint).Address.ToString(), _id++);
                }
                Connection.BeginAccept(AcceptCallback, null);
                SocketArgs args = new SocketArgs(accepted);
                Accept?.Invoke(this, args);
                accepted.BeginReceive(_rcvBuffer, 0, GeneralBufferSize, SocketFlags.None, ReceiveCallback, accepted);
            }
            catch
            {
                return;
            }
        }
        private void ReceiveCallback(IAsyncResult ar)
        {
            Socket sock = ar.AsyncState as Socket;

            int receivedBytes = 0;
            try
            {
                receivedBytes = sock.EndReceive(ar);
            }
            catch
            {
                sock.Close();
                Connections.Remove(sock);
                return;
            }

            TransferArgs args = new TransferArgs(sock, _rcvBuffer, receivedBytes);
            Receive?.Invoke(this, args);
            sock.BeginReceive(_rcvBuffer, 0, GeneralBufferSize, SocketFlags.None, (ReceiveCallback), sock);
        }
    }
}
