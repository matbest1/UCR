using Sodium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using UCK;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    internal class Client
    {
        public static ClashKeyPair GenerateKeyPair()
        {
            var keyPair = PublicKeyBox.GenerateKeyPair();
            return new ClashKeyPair(keyPair.PublicKey, keyPair.PrivateKey);
        }

        private Level m_vLevel;
        private readonly long m_vSocketHandle;

        public Client(Socket so)
        {
            Socket = so;
            m_vSocketHandle = so.Handle.ToInt64();
            DataStream = new List<byte>();
            CState = 0;
        }

        public int ClientSeed { get; set; }
        public byte[] CPublicKey { get; set; }
        public byte[] CSessionKey { get; set; }
        public byte[] CSNonce { get; set; }
        public int CState { get; set; }
        public string CIPAddress { get; set; }
        public byte[] CRNonce { get; set; }
        public byte[] CSharedKey { get; set; }
        public List<byte> DataStream { get; set; }
        public byte[] IncomingPacketsKey { get; set; }
        public byte[] OutgoingPacketsKey { get; set; }
        public Socket Socket { get; set; }

        public static byte[] GenerateSessionKey()
        {
            return PublicKeyBox.GenerateNonce();
        }

        public Level GetLevel()
        {
            return m_vLevel;
        }

        public long GetSocketHandle()
        {
            return m_vSocketHandle;
        }

        public bool IsClientSocketConnected()
        {
            try
            {
                return !((Socket.Poll(1000, SelectMode.SelectRead) && (Socket.Available == 0)) || !Socket.Connected);
            }
            catch
            {
                return false;
            }
        }

        public void SetLevel(Level l)
        {
            m_vLevel = l;
        }

        public bool TryGetPacket(out Message p)
        {
            p = null;
            var result = false;
            if (DataStream.Count() >= 5)
            {
                var length = (0x00 << 24) | (DataStream[2] << 16) | (DataStream[3] << 8) | DataStream[4];
                var type = (ushort)((DataStream[0] << 8) | DataStream[1]);
                if (DataStream.Count - 7 >= length)
                {
                    object obj = null;
                    var packet = DataStream.Take(7 + length).ToArray();
                    using (var br = new BinaryReader(new MemoryStream(packet)))
                    {
                        obj = MessageFactory.Read(this, br, type);
                    }
                    if (obj != null)
                    {
                        p = (Message)obj;
                        result = true;
                    }
                    else
                    {
                        var data = DataStream.Skip(7).Take(length).ToArray();
                    }
                    DataStream.RemoveRange(0, 7 + length);
                }
            }
            return result;
        }
    }
}