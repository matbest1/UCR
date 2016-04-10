using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration;
using UCS.Helpers;
using UCS.Sys;
using System.IO;

namespace UCS.Core.Threading
{
    class ConsoleThread
    {
        /// <summary>
        /// Variable holding the thread itself
        /// </summary>
        private static Thread T { get; set; }

        /// <summary>
        /// Starts the Thread
        /// </summary>
        [STAThread]
        public void Start()
        {
            T = new Thread(() =>
            {
                if (ConfUCS.IsConsoleMode) Console.Title = ConfUCS.UnivTitle;
                CancelEvent(); //N00b proof
                /* ASCII Art centered */
                Console.WriteLine(
                    @"
    888     888 888    88888888888 8888888b.         d8888 8888888b.   .d88888b.  888       888        d8888 
    888     888 888        888     888   Y88b       d88888 888   Y88b d88P' 'Y88b 888   o   888       d88888 
    888     888 888        888     888    888      d88P888 888    888 888     888 888  d8b  888      d88P888
    888     888 888        888     888   d88P     d88P 888 888   d88P 888     888 888 d888b 888     d88P 888
    888     888 888        888     8888888P'     d88P  888 8888888P'  888     888 888d88888b888    d88P  888
    888     888 888        888     888 T88b     d88P   888 888        888     888 88888P Y88888   d88P   888
    Y88b. .d88P 888        888     888  T88b   d8888888888 888        Y88b. .d88P 8888P   Y8888  d8888888888
     'Y88888P'  88888888   888     888   T88b d88P     888 888         'Y88888P'  888P     Y888 d88P     888
                  ");

                Console.WriteLine("[UCS]    -> This Program is made by the Ultrapowa Network Developer Team!");
                Console.WriteLine("[UCs]    -> You can find the source at www.ultrapowa.com and www.github.com/ultrapowa/ucs");
                Console.WriteLine("[UCS]    -> Don't forget to visit www.ultrapowa.com daily for news update !");
                Console.WriteLine("[UCS]    -> UCS is now starting...");
                Preload PT = new Preload();
                PT.PreloadThings();
                ControlTimer.StartPerformanceCounter();
                Console.WriteLine("");
                if (!Directory.Exists("logs"))
                    Directory.CreateDirectory("logs");
                Debugger.SetLogLevel(int.Parse(ConfigurationManager.AppSettings["loggingLevel"]));
                Logger.SetLogLevel(int.Parse(ConfigurationManager.AppSettings["loggingLevel"]));
                NetworkThread.Start();
                MemoryThread.Start();
                ConfUCS.UnivTitle = "Ultrapowa Clash Server " + ConfUCS.VersionUCS + " | " + "ONLINE";
                if (ConfUCS.IsConsoleMode) CommandParser.ManageConsole();
            });
            T.SetApartmentState(ApartmentState.STA);
            T.Start();
        }

        private void CancelEvent()
        {
            var exitEvent = new ManualResetEvent(false);

            Console.CancelKeyPress += (sender, e) => {
                e.Cancel = true;
                exitEvent.Set();
            };
        }

        /// <summary>
        /// Stops the Thread
        /// </summary>
        public static void Stop()
        {
            if (T.ThreadState == ThreadState.Running)
                T.Abort();
        }
    }
}
