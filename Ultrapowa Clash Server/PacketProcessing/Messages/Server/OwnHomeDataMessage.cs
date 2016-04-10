using System;
using System.Collections.Generic;
using System.Linq;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    //Packet 24101
    internal class OwnHomeDataMessage : Message
    {
        public OwnHomeDataMessage(Client client, Level level) : base(client)
        {
            SetMessageType(24101);
        }

        public override void Encode()
        {
            var data = new List<byte>();

            Encrypt(data.ToArray());
        }
    }
}