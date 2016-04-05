using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using UCS.Core;
using UCS.Network;
using UCS.PacketProcessing;

namespace UCS.Helpers
{
    internal class CommandParser
    {
        public static void Parse(string Command)
        {
            switch (Command)
            {
                case "/help":
                    Console.WriteLine("[UCR][MENU]  -> /startx      - Starts the UCR Interface.");
                    Console.WriteLine("[UCR][MENU]  -> /status      - Shows the actual UCR status.");
                    Console.WriteLine("[UCR][MENU]  -> /clear       - Clears the console screen.");
                    Console.WriteLine("[UCR][MENU]  -> /restart     - Restarts UCR instantly.");
                    Console.WriteLine("[UCR][MENU]  -> /shutdown    - Shuts UCR down instantly.");
                    break;

                case "/status":
                    Console.WriteLine("");
                    Console.WriteLine("[UCR][INFO]  -> IP Address (public):    " + new WebClient().DownloadString("http://bot.whatismyipaddress.com/"));
                    Console.WriteLine("[UCR][INFO]  -> IP Address (local):     " + Dns.GetHostByName(Dns.GetHostName()).AddressList[0]);
                    Console.WriteLine("[UCR][INFO]  -> Online players:         " + ResourcesManager.GetOnlinePlayers().Count);
                    Console.WriteLine("[UCR][INFO]  -> Connected players:      " + ResourcesManager.GetConnectedClients().Count);
                    Console.WriteLine("[UCR][INFO]  -> Clash Royale Version:          " + ConfigurationManager.AppSettings["clientVersion"]);
                    break;

                case "/clear":
                    Console.Clear();
                    break;

                case "/restart":
                    Process.Start(Application.ExecutablePath);
                    Environment.Exit(0);
                    break;

                case "/shutdown":
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("[UCR]    Unknown command, type \"/help\" for a list containing all available commands.");
                    break;
            }
        }
    }
}