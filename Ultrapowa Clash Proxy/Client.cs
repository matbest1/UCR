using System;
using System.Net;
using System.Net.Sockets;

namespace UCP
{
    public class Client : ClientCrypto
    {
        public ClientState state = new ClientState();

        public Client(ServerState serverstate)
        {
            state.serverState = serverstate;
            state.clientKey = clientKey;
            state.serverKey = serverKey;
        }

        public void StartClient()
        {
            try
            {
                var ipHostInfo = Dns.GetHostEntry(UCP.Proxy.hostname);
                var ipAddress = ipHostInfo.AddressList[0];
                var remoteEndPoint = new IPEndPoint(ipAddress, UCP.Proxy.port);

                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                state.socket = socket;
                socket.Connect(remoteEndPoint);
                socket.BeginReceive(state.buffer, 0, State.BufferSize, 0, ReceiveCallback, state);

                Console.WriteLine("[UCS][INFO]  The proxy is succefully linked to {0} ...", socket.RemoteEndPoint);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}