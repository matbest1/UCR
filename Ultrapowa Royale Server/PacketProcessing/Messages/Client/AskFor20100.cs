using System.IO;
using System;
using UCS.Helpers;
using UCS.Logic;
using UCS.Network;

namespace UCS.PacketProcessing
{
    internal class Askfor20100 : Message
    {
        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;
        /// <summary>
        /// Unknown integer 2.
        /// </summary>
        public int Unknown2;
        /// <summary>
        /// Unknown integer 3.
        /// </summary>
        public int Unknown3;
        /// <summary>
        /// Unknown integer 4.
        /// </summary>
        public int Unknown4;
        /// <summary>
        /// Unknown integer 5.
        /// </summary>
        public int Unknown5;
        /// <summary>
        /// String that is probably needed for the new encryption
        /// schema.
        /// </summary>
        public string TheString;
        /// <summary>
        /// Unknown integer 6.
        /// </summary>
        public int Unknown6;
        /// <summary>
        /// Unknown integer 7.
        /// </summary>
        public int Unknown7;

        public Askfor20100(Client client, BinaryReader br) : base(client, br)
        {
            //Not sure if there should be something here o.O
        }

        public override void Decode()
        {
            using (var br = new BinaryReader(new MemoryStream(GetData())))
            {
                Unknown1 = br.ReadInt32();
                Unknown2 = br.ReadInt32();
                Unknown3 = br.ReadInt32();
                Unknown4 = br.ReadInt32();
                Unknown5 = br.ReadInt32();
                Unknown6 = br.ReadInt32();
                TheString = br.ReadScString();
                Unknown7 = br.ReadInt32();

            }
        }

        public override void Process(Level level)
        {
            Console.WriteLine(Unknown1);
            Console.WriteLine(Unknown2);
            Console.WriteLine(Unknown3);
            Console.WriteLine(Unknown4);
            Console.WriteLine(Unknown5);
            Console.WriteLine(Unknown6);
            Console.WriteLine(Unknown7);
            var p = new LoginFailedMessage(Client);
            p.SetErrorCode(10);
            p.RemainingTime(0);
            p.SetReason("You are connecting with a 8.67 Client but UCS doesn't support it yet.");
            PacketManager.ProcessOutgoingPacket(p);
        }
    }
}