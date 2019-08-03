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
        private byte[] _sndBuffer;

        public Server(string host, int port, int maxBufferSize = 1024)
        {
            _rcvBuffer = new byte[maxBufferSize];
            _sndBuffer = new byte[maxBufferSize];
            Connections = new List<Socket>();
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
        public int GeneralBufferSize { get; private set; }

        public event ConnectionEventHandler Accept;
        public event ConnectionEventHandler Send;
        public event ConnectionEventHandler Receive;
        public event ConnectionEventHandler Broadcast;
        public event ConnectionEventHandler Disconnect;

        private void AcceptCallback(IAsyncResult ar)
        {
            Socket accepted;
            try
            {
                accepted = Connection.EndAccept(ar);
                Connections.Add(accepted);
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

        public void BeginSend(Socket sock, byte[] buffer, SocketFlags flags = SocketFlags.None)
        {
            _sndBuffer = buffer;
            sock.BeginSend(buffer, 0, buffer.Length, flags, new AsyncCallback(SendCallback), sock);
        }

        private void SendCallback(IAsyncResult ar)
        {
            Socket sock = ar.AsyncState as Socket;
            sock.EndSend(ar);

            TransferArgs args = new TransferArgs(sock, _sndBuffer, _sndBuffer.Length);
            Send?.Invoke(this, args);
        }
    }
}
