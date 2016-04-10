using System;

namespace UCP
{
    public class Protocol
    {
        protected static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                var state = (State)ar.AsyncState;
                var socket = state.socket;
                var bytesReceived = socket.EndReceive(ar);
                PacketReceiver.receive(bytesReceived, socket, state);
                socket.BeginReceive(state.buffer, 0, State.BufferSize, 0, ReceiveCallback, state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        protected static void SendCallback(IAsyncResult ar)
        {
            try
            {
                var state = (State)ar.AsyncState;
                var socket = state.socket;
                var bytesSent = socket.EndSend(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}