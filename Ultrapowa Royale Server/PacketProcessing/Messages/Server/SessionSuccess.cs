using System.Collections.Generic;
using System.IO;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Packet 20100
    internal class SessionSuccess : Message
    {
        public byte[] SessionKey;

        public SessionSuccess(Client client, SessionRequest cka) : base(client)
        {
            SetMessageType(20100);
            SessionKey = Client.GenerateSessionKey();
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddInt32(SessionKey.Length);
            pack.AddRange(SessionKey);
            SetData(pack.ToArray());
        }
    }
}