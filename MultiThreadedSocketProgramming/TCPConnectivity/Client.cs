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
    public class Client
    {
        private byte[] _rcvBuffer;
        private byte[] _sndBuffer;

        public Client(string host, int port, int bufferSize = 1024)
        {
            GeneralBufferSize = bufferSize;
            IPEndPoint = new IPEndPoint(IPAddress.Parse(host), port);
            Connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Connection.BeginConnect(IPEndPoint, ConnectCallback, Connection);

            _rcvBuffer = new byte[1024];

            var buffer = new byte[bufferSize];
            Connection.BeginReceive(buffer, 0, bufferSize, SocketFlags.None, ReceiveCallback, Connection);
        }

        public int GeneralBufferSize { get; set; }
        public Socket Connection { get; set; }
        public IPEndPoint IPEndPoint { get; set; }

        public event ConnectionEventHandler Connect;
        public event ConnectionEventHandler Send;
        public event ConnectionEventHandler Receive;
        public event ConnectionEventHandler Disconnect;


        private void ConnectCallback(IAsyncResult ar)
        {
            Connection.EndConnect(ar);

            SocketArgs args = new SocketArgs(Connection);
            Connect?.Invoke(this, args);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            int receiveBytes = Connection.EndReceive(ar);
            if (receiveBytes > 0)
            {
                Connection.BeginReceive(_rcvBuffer, 0, GeneralBufferSize, SocketFlags.None, ReceiveCallback, Connection);
                TransferArgs args = new TransferArgs(Connection, _rcvBuffer, receiveBytes);
                Receive?.Invoke(this, args);
            }
        }

        public void BeginSend(byte[] buffer, SocketFlags flags = SocketFlags.None)
        {
            _sndBuffer = buffer;
            Connection.BeginSend(buffer, 0, buffer.Length, flags, new AsyncCallback(SendCallback), Connection);
        }

        private void SendCallback(IAsyncResult ar)
        {
            Connection.EndSend(ar);
            TransferArgs args = new TransferArgs(Connection, _sndBuffer, _sndBuffer.Length);
            Send?.Invoke(this, args);
        }

        public void BeginDisconnect()
        {
            Connection.BeginDisconnect(false, DisconnectCallback, null);
        }

        private void DisconnectCallback(IAsyncResult ar)
        {
            Connection.EndDisconnect(ar);
            SocketArgs args = new SocketArgs(Connection);
            Disconnect?.Invoke(this, args);
        }
    }
}
