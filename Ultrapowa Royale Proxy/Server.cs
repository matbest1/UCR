using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace UCP
{
    public class Server : ServerCrypto
    {
        private static readonly ManualResetEvent allDone = new ManualResetEvent(false);
        private readonly int port;

        public Server(int port)
        {
            this.port = port;
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();
            try
            {
                var listener = (Socket)ar.AsyncState;
                var socket = listener.EndAccept(ar);

                var state = new ServerState
                {
                    socket = socket,
                    serverKey = serverKey
                };

                Console.WriteLine("[UCR]    Connection from {0} ...", socket.RemoteEndPoint);

                var client = new Client(state);
                client.StartClient();
                client.state.serverState = state;
                state.clientState = client.state;

                socket.BeginReceive(state.buffer, 0, State.BufferSize, 0, ReceiveCallback, state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void StartServer()
        {
            try
            {
                var localEndPoint = new IPEndPoint(IPAddress.Any, port);
                using (var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    listener.Bind(localEndPoint);
                    listener.Listen(100);

                    Console.WriteLine("[UCR]    Started Listener on Port {0} ...", port);

                    while (true)
                    {
                        allDone.Reset();
                        listener.BeginAccept(AcceptCallback, listener);
                        allDone.WaitOne();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[UCR]    {0}", e.Message);
            }
            Console.WriteLine("\n[UCR]      Press ENTER to continue...");
            Console.Read();
        }
    }
}