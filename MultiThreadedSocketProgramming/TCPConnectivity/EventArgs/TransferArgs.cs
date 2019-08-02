using System.Net.Sockets;

namespace TCPConnectivity.EventArgs
{
    public class TransferArgs : SocketArgs
    {
        public TransferArgs(Socket sock, byte[] data, int size)
            : base(sock)
        {
            Data = data;
            Size = size;
        }

        public byte[] Data { get; protected set; }
        public int Size { get; protected set; }

        public byte[] Get()
        {
            var bytes = new byte[Size];
            for (int i = 0; i < Size; i++)
            {
                bytes[i] = Data[i];
            }
            return bytes;
        }
    }
}
