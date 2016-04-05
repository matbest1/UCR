using System;
using System.Configuration;
using System.Threading;
using UCS.Network;

namespace UCS.Core.Threading
{
    internal class NetworkThread
    {
        private static Thread T { get; set; }

        public static void Start()
        {
            T = new Thread(() =>
            {
                new ResourcesManager();
                new ObjectManager();
                new PacketManager().Start();
                new MessageManager().Start();
                new Gateway().Start();
                Console.WriteLine("[UCR]    Server started, let's play Clash Royale!");
            });
            T.Start();
        }

        public static void Stop()
        {
            if (T.ThreadState == ThreadState.Running)
                T.Abort();
        }
    }
}