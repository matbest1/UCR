using Sodium;
using System.IO;
using UCS.Logic;
using UCS.Network;

namespace UCS.PacketProcessing
{
    internal class KeepAliveMessage : Message
    {
        public KeepAliveMessage(Client client, BinaryReader br) : base(client, br)
        {
            Decrypt();
        }

        public override void Process(Level level)
        {
            PacketManager.ProcessOutgoingPacket(new KeepAliveOkMessage(Client, this));
        }
    }
}