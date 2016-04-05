using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UCS.Core;
using UCS.PacketProcessing;

namespace UCS.Network
{
    internal class Gateway
    {
        private const int kHostConnectionBacklog = 30;
        private const int kPort = 9339;
        private IPAddress ip;

        public static Socket Socket { get; private set; }

        public IPAddress IP
        {
            get
            {
                if (ip == null)
                {
                    ip = (
                        from entry in Dns.GetHostEntry(Dns.GetHostName()).AddressList
                        where entry.AddressFamily == AddressFamily.InterNetwork
                        select entry
                        ).FirstOrDefault();
                }
                return ip;
            }
        }

        public bool Host(int port)
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                Socket.Bind(new IPEndPoint(IPAddress.Any, port));
                Socket.Listen(kHostConnectionBacklog);
                Socket.BeginAccept(OnClientConnect, Socket);
            }
            catch (Exception e)
            {
                Console.WriteLine("[UCR] Exception when attempting to host (" + port + "): " + e);
                Socket = null;
                return false;
            }
            return true;
        }

        public void Start()
        {
            if (Host(kPort))
                Console.WriteLine("[UCR]    Gateway started on port " + kPort);
        }

        private static void Disconnect()
        {
            if (Socket != null)
                Socket.BeginDisconnect(false, OnEndHostComplete, Socket);
        }

        private void OnClientConnect(IAsyncResult result)
        {
            try
            {
                var clientSocket = Socket.EndAccept(result);
                ResourcesManager.AddClient(new Client(clientSocket), ((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString());
                SocketRead.Begin(clientSocket, OnReceive, OnReceiveError);
                Console.WriteLine("[UCR]    Client connected (" + ((IPEndPoint)clientSocket.RemoteEndPoint).Address + ":" +
                                  ((IPEndPoint)clientSocket.RemoteEndPoint).Port + ")");
            }
            catch (Exception e)
            {
                Console.WriteLine("[UCR]    Exception when accepting incoming connection: " + e);
            }
            try
            {
                Socket.BeginAccept(OnClientConnect, Socket);
            }
            catch (Exception e)
            {
                Console.WriteLine("[UCR]    Exception when starting new accept process: " + e);
            }
        }

        private static void OnEndHostComplete(IAsyncResult result)
        {
            Socket = null;
        }

        private static void OnReceive(SocketRead read, byte[] data)
        {
            try
            {
                var socketHandle = read.Socket.Handle.ToInt64();
                var c = ResourcesManager.GetClient(socketHandle);
                c.DataStream.AddRange(data);

                Message p;
                while (c.TryGetPacket(out p))
                    PacketManager.ProcessIncomingPacket(p);
            }
            catch (Exception ex)
            {
                Debugger.WriteLine("[UCR]   Exception thrown when processing incoming packet : ", ex);
            }
        }

        private static void OnReceiveError(SocketRead read, Exception exception)
        {
        }
    }
}