using System.Net.Sockets;

namespace UCP
{
    public class State
    {
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
        public byte[] packet = new byte[0];
        public Socket socket = null;
    }
}