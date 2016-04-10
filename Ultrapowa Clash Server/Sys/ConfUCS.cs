using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Threading;
using UCS.Core;

namespace UCS.Sys
{
    public class ConfUCS
    {
        public static Version thisAppVer = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        public static string Language = "en-US"; //Translation coming soon.
        public static bool DebugMode = true;
        public static bool EnableRemoteControl = false;
        public static bool IsConsoleMode = true; //Will start console first, overridable by config overridable by args.
        public static bool IsDefaultMode = false;
        public static bool IsLogEnabled = true;
        public static string LogDirectory = System.IO.Directory.GetCurrentDirectory() + @"\logs\";
        public static readonly string VersionUCS = "" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        public static string UnivTitle = "Ultrapowa Clash Server " + VersionUCS +  " | " + "OFFLINE";
        public static string LogPath = "NONE";
        public static bool IsPreloaded = false;
        public static bool IsConsoleFirst = false;
        public static PluginManager PM = new PluginManager();

        public static bool _IsServerOnline = false;
        public static bool IsServerOnline
        {
            get { return _IsServerOnline; }
            set
            {
                _IsServerOnline = value;
                ControlTimer.StartUpTimer();
                OnServerOnlineChange();
            }
        }
        public static bool AutoStartServer = true;


        //Updater section
        public static bool IsUpdateAvailable = false;
        public static Version NewVer = new Version();
        public static string UrlXML = "http://www.google.com";
        public static string UrlPage = "http://www.ultrapowa.com";
        public static string Changelog = "Error, changelog not downloaded...";

        public static string GetIP()
        {
            string HostName = Dns.GetHostName();
            return Dns.GetHostByName(HostName).AddressList[0].ToString();
        }

        public static event EventHandler OnServerOnlineEvent;

        public static void OnServerOnlineChange()
        {
            if (OnServerOnlineEvent != null) OnServerOnlineEvent(typeof(ConfUCS), EventArgs.Empty);
        }

        public void PreloadConfig()
        {
            //Here will preload and check for config errors.
        }

    }
}
