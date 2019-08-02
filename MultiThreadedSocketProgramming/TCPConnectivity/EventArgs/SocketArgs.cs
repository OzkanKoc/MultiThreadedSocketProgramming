using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPConnectivity.EventArgs
{
    public class SocketArgs : System.EventArgs
    {
        public SocketArgs(Socket sock)
        {
            Connection = sock;
        }

        public Socket Connection { get; protected set; }
    }
}
