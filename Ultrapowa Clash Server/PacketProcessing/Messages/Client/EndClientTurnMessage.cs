using System;
using System.IO;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    //Packet 14102

    internal class EndClientTurnMessage : Message
    {
        public EndClientTurnMessage(Client client, BinaryReader br) : base(client, br)
        {
            Decrypt();
        }

        public override void Decode()
        {

        }

        public override void Process(Level level)
        {

        }
    }
}