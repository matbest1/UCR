using System;
using System.Configuration;
using System.Threading;
using UCS.Network;
using UCS.Sys;

namespace UCS.Core.Threading
{
    internal class NetworkThread
    {

        /// <summary>
        /// Variable holding the thread itself
        /// </summary>
        private static Thread T { get; set; }

        public static int ParseConfigInt(string str) => int.Parse(ConfigurationManager.AppSettings[str]);

        public static string parseConfigString(string str) => ConfigurationManager.AppSettings[str];

        public static void Start()
        {
            T = new Thread(() =>
            {
                new ResourcesManager();
                new ObjectManager();
                new PacketManager().Start();
                new MessageManager().Start();
                new Gateway().Start();
                //HTTP API = new HTTP(Convert.ToInt32(ConfigurationManager.AppSettings["proDebugPort"]));
                //new UCSList();
                ControlTimer.StopPerformanceCounter();
                ControlTimer.Setup();
                ConfUCS.IsServerOnline = true;
                Console.WriteLine("[UCS]    Server started, let's play Clash of Clans!");
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