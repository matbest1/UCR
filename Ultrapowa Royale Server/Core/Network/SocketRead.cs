using System;
using System.Net.Sockets;

//using System.Collections.Generic;

namespace UCS.Network
{
    public class SocketRead
    {
        public delegate void IncomingReadErrorHandler(SocketRead read, Exception exception);

        public delegate void IncomingReadHandler(SocketRead read, byte[] data);

        public const int kBufferSize = 256;

        private readonly byte[] buffer = new byte[kBufferSize];

        private readonly IncomingReadErrorHandler errorHandler;

        private readonly IncomingReadHandler readHandler;

        private SocketRead(Socket socket, IncomingReadHandler readHandler, IncomingReadErrorHandler errorHandler = null)
        {
            Socket = socket;
            this.readHandler = readHandler;
            this.errorHandler = errorHandler;
            BeginReceive();
        }

        public Socket Socket { get; }

        public static SocketRead Begin(Socket socket, IncomingReadHandler readHandler, IncomingReadErrorHandler errorHandler = null)
        {
            return new SocketRead(socket, readHandler, errorHandler);
        }

        private void BeginReceive()
        {
            Socket.BeginReceive(buffer, 0, kBufferSize, SocketFlags.None, OnReceive, this);
        }

        private void OnReceive(IAsyncResult result)
        {
            try
            {
                if (result.IsCompleted)
                {
                    var bytesRead = Socket.EndReceive(result);
                    if (bytesRead > 0)
                    {
                        var read = new byte[bytesRead];
                        Array.Copy(buffer, 0, read, 0, bytesRead);

                        readHandler(this, read);
                        Begin(Socket, readHandler, errorHandler);
                    }
                }
            }
            catch (Exception e)
            {
                if (errorHandler != null)
                    errorHandler(this, e);
            }
        }
    }
}