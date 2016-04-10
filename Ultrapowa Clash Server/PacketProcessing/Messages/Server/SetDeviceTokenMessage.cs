using System.Collections.Generic;
using System.Linq;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    //Packet 20113
    internal class SetDeviceTokenMessage : Message
    {
        public string UserToken { get; set; }

        public SetDeviceTokenMessage(Client client) : base(client)
        {
            SetMessageType(20113);
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddString(UserToken);
            Encrypt(pack.ToArray());
        }
    }
}