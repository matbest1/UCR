using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Windows;
using UCS.Core;
using UCS.Core.Interfaces;
using UCS.Core.Threading;
using UCS.Logic;
using UCS.Network;
using UCS.PacketProcessing;
using UCS.Sys;

namespace UCS.Helpers
{
    class CommandParser
    {
        public static string GetCommand { get; private set; }

        private static bool CommandFound;

        public static void CommandRead(string cmd)
        {
            if (cmd == null) if (ConfUCS.IsConsoleMode) ManageConsole(); //Prevent useless bugs
            var subCommand = cmd.Split(' ');

            try
            {
                for (int i = 0; i < ConfUCS.PM.LoadedPluginsICP.Count; i++)
                {
                    for (int j = 0; j < ConfUCS.PM.LoadedPluginsICP[i].plugin.CommandList().Count; j++)
                    {
                        if (ConfUCS.PM.LoadedPluginsICP[i].plugin.CommandList()[j].Command == subCommand[0])
                        {
                            if (ConfUCS.PM.LoadedPluginsICP[i].plugin.CommandList()[j].StartWithString)
                                GetCommand = cmd;
                            ConfUCS.PM.LoadedPluginsICP[i].plugin.CommandList()[j].ExecuteCommand();

                            CommandFound = true;

                            //So will skip all remanings command to find to improve performance
                            i = ConfUCS.PM.LoadedPluginsICP.Count - 1;
                            j = ConfUCS.PM.LoadedPluginsICP[i].plugin.CommandList().Count - 1;
                        }
                    }
                }
                if (!CommandFound) Console.WriteLine("Unknown command. Type \"/help\" for a list containing all available commands.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something wrong happens...");
                Console.WriteLine(ex.Message);
            }

            //Cleanup sheeps
            GetCommand = "";
            CommandFound = false;

           if (ConfUCS.IsConsoleMode) ManageConsole();
        }

        public static void ManageConsole() => CommandRead(Console.ReadLine());
    }

    class CoreCommand : ICommandPlugin
    {
        public string AuthorName => "UCS";
        public string ImageURL => "";
        public string Information => "Command Handler";
        public string Title => "UCS Core";
        public string URL => "http://www.ultrapowa.com";
        public string Version => ConfUCS.VersionUCS;

        public List<CommandActionList> CommandList()
        {
            var CAL = new List<CommandActionList>();
            CAL.Add(new CommandActionList() { Command = "/help", ExecuteCommand = HelpCommand });
            CAL.Add(new CommandActionList() { Command = "/start", ExecuteCommand = StartCommand });
            CAL.Add(new CommandActionList() { Command = "/stop", ExecuteCommand = StopCommand });
            CAL.Add(new CommandActionList() { Command = "/shutdown", ExecuteCommand = StopCommand });
            CAL.Add(new CommandActionList() { Command = "/forcestop", ExecuteCommand = ForceStopCommand });
            CAL.Add(new CommandActionList() { Command = "/uptime", ExecuteCommand = UptimeCommand });
            CAL.Add(new CommandActionList() { Command = "/restart", ExecuteCommand = RestartCommand });
            CAL.Add(new CommandActionList() { Command = "/clear", ExecuteCommand = ClearCommand });
            CAL.Add(new CommandActionList() { Command = "/status", ExecuteCommand = StatusCommand });
            CAL.Add(new CommandActionList() { Command = "/send sysinfo", ExecuteCommand = SendSysInfoCommand });
            CAL.Add(new CommandActionList() { Command = "/switch", ExecuteCommand = SwitchCommand });
            CAL.Add(new CommandActionList() { Command = "/plugins", ExecuteCommand = PluginsCommand });
            CAL.Add(new CommandActionList() { StartWithString = true, Command = "/kick", ExecuteCommand = KickCommand });
            CAL.Add(new CommandActionList() { StartWithString = true, Command = "/ban", ExecuteCommand = BanCommand });
            CAL.Add(new CommandActionList() { StartWithString = true, Command = "/unban", ExecuteCommand = UnbanCommand });
            CAL.Add(new CommandActionList() { StartWithString = true, Command = "/say", ExecuteCommand = SayCommand });
            return CAL;
        }

        private void HelpCommand()
        {
            Console.WriteLine("[UCS]   /start                             <-- Start the server");
            Console.WriteLine("[UCS]   /stop  or   /shutdown              <-- Stop the server and save data");
            Console.WriteLine("[UCS]   /forcestop                         <-- Force stop the server");
            Console.WriteLine("[UCS]   /uptime                            <-- Get server uptime");
            Console.WriteLine("[UCS]   /restart                           <-- Save data and then restart");
            Console.WriteLine("[UCS]   /clear                             <-- Clear the console");
            Console.WriteLine("[UCS]   /status                            <-- Get server status");
            Console.WriteLine("[UCS]   /send sysinfo                      <-- Send server info to all players");
            Console.WriteLine("[UCS]   /switch                            <-- Switch to GUI/Console mode");
            Console.WriteLine("[UCS]   /switch                            <-- Get list of plugins");
            Console.WriteLine("[UCS]   /say <Text>                        <-- Send a text to all");
            Console.WriteLine("[UCS]   /kick <PlayerID>                   <-- Kick a client from the server");
            Console.WriteLine("[UCS]   /ban <PlayerID>                    <-- Ban a client");
            Console.WriteLine("[UCS]   /unban <PlayerID>                  <-- Unban a client");
            
            // Console.WriteLine("/banip <PlayerID>                  <-- Ban a client by IP");
            // Console.WriteLine("/unbanip <PlayerID>                <-- Unban a client");
            // Console.WriteLine("/tempban <PlayerID> <Seconds>      <-- Temporary ban a client");
            // Console.WriteLine("/tempbanip <PlayerID> <Seconds>    <-- Temporary ban a client by IP");
            // Console.WriteLine("/mute <PlayerID>                   <-- Mute a client");
            // Console.WriteLine("/unmute <PlayerID>                 <-- Unmute a client");
            // Console.WriteLine("/setlevel <PlayerID> <Level>       <-- Set a level for a player");
            // Console.WriteLine("/update                            <-- Check if update is available");          
            // Console.WriteLine("/sayplayer <PlayerID> <Text>       <-- Send a text to a player");
            

        }
        private void StartCommand()
        {
            if (!ConfUCS.IsServerOnline)
            {
                ConsoleThread CT = new ConsoleThread();
                CT.Start();
            }
            else Console.WriteLine("[UCS]   Server already online!");
        }
        private void StopCommand()
        {
            Console.WriteLine("[UCS]   Shutting down... Saving all data, wait.");
            /*
            foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
            {
                var p = new ShutdownStartedMessage(onlinePlayer.GetClient());
                p.SetCode(5);
                PacketManager.ProcessOutgoingPacket(p);
            }*/

            ConsoleManage.FreeConsole();
            Environment.Exit(0);
        }
        private void ForceStopCommand()
        {
            Console.WriteLine("[UCS]   Force shutting down... All progress not saved will be lost!");
            Process.GetCurrentProcess().Kill();
        }
        private void UptimeCommand()
        {
            Console.WriteLine("[UCS]   Up time: " + ControlTimer.ElapsedTime);
        }
        private void RestartCommand()
        {
            Console.WriteLine("[UCS]   System Restarting....");

            var mail = new AllianceMailStreamEntry();
            mail.SetId((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            mail.SetSenderId(0);
            mail.SetSenderAvatarId(0);
            mail.SetSenderName("System Manager");
            mail.SetIsNew(0);
            mail.SetAllianceId(0);
            mail.SetAllianceBadgeData(0);
            mail.SetAllianceName("Legendary Administrator");
            mail.SetMessage("System is about to restart in a few moments.");
            mail.SetSenderLevel(500);
            mail.SetSenderLeagueId(22);
            /*
            foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
            {
                var pm = new GlobalChatLineMessage(onlinePlayer.GetClient());
                var ps = new ShutdownStartedMessage(onlinePlayer.GetClient());
                var p = new AvatarStreamEntryMessage(onlinePlayer.GetClient());
                ps.SetCode(5);
                p.SetAvatarStreamEntry(mail);
                pm.SetChatMessage("System is about to restart in a few moments.");
                pm.SetPlayerId(0);
                pm.SetLeagueId(22);
                pm.SetPlayerName("System Manager");
                PacketManager.ProcessOutgoingPacket(p);
                PacketManager.ProcessOutgoingPacket(ps);
                PacketManager.ProcessOutgoingPacket(pm);
            }*/
            Console.WriteLine("Saving all data...");
            foreach (var l in ResourcesManager.GetOnlinePlayers())
            {
                //DatabaseManager.Singelton.Save(l);
            }

            Console.WriteLine("Restarting now");

            Process.Start(Application.ResourceAssembly.Location);
            Process.GetCurrentProcess().Kill();
        }
        private void ClearCommand()
        {
            if (ConfUCS.IsConsoleMode)
                Console.Clear();
            else
                ConsoleStreamer.Value = "";
            Console.WriteLine("[UCS]   Console cleared");
        }
        private void StatusCommand()
        {
            Console.WriteLine("[UCS]   Server IP: " + ConfUCS.GetIP() + " on port 9339");
            Console.WriteLine("[UCS]   IP Address (public): " + new WebClient().DownloadString("http://bot.whatismyipaddress.com/"));
            Console.WriteLine("[UCS]   Online Player: " + ResourcesManager.GetOnlinePlayers().Count);
            Console.WriteLine("[UCS]   Connected Player: " + ResourcesManager.GetConnectedClients().Count);
            Console.WriteLine("[UCS]   Starting Gold: " + int.Parse(ConfigurationManager.AppSettings["StartingGold"]));
            Console.WriteLine("[UCS]   Starting Elixir: " +
                              int.Parse(ConfigurationManager.AppSettings["StartingElixir"]));
            Console.WriteLine("[UCS]   Starting Dark Elixir: " +
                              int.Parse(ConfigurationManager.AppSettings["StartingDarkElixir"]));
            Console.WriteLine("[UCS]   Starting Gems: " + int.Parse(ConfigurationManager.AppSettings["StartingGems"]));
            Console.WriteLine("[UCS]   CoC Version: " + ConfigurationManager.AppSettings["ClientVersion"]);

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["useCustomPatch"]))
            {
                Console.WriteLine("[UCS]   Patch: Active");
                Console.WriteLine("[UCS]   Patching Server: " + ConfigurationManager.AppSettings["patchingServer"]);
            }
            else
                Console.WriteLine("[UCS]   Patch: Disable");

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["maintenanceMode"]))
            {
                Console.WriteLine("[UCS]   Maintance Mode: Active");
                Console.WriteLine("[UCS]   Maintance time: " + Convert.ToInt32(ConfigurationManager.AppSettings["maintenanceTimeleft"]) + " Seconds");
            }
            else
                Console.WriteLine("[UCS]   Maintance Mode: Disable");
        }
        private void SendSysInfoCommand()
        {
            Console.WriteLine("[UCS]   Server Status is now sent to all online players");

            var mail1 = new AllianceMailStreamEntry();
            mail1.SetId((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            mail1.SetSenderId(0);
            mail1.SetSenderAvatarId(0);
            mail1.SetSenderName("System Manager");
            mail1.SetIsNew(0);
            mail1.SetAllianceId(0);
            mail1.SetAllianceBadgeData(0);
            mail1.SetAllianceName("Legendary Administrator");
            mail1.SetMessage("Latest Server Status:\nConnected Players:" +
                            ResourcesManager.GetConnectedClients().Count + "\nIn Memory Alliances:" +
                            ObjectManager.GetInMemoryAlliances().Count + "\nIn Memory Levels:" +
                            ResourcesManager.GetInMemoryLevels().Count);
            mail1.SetSenderLeagueId(22);
            mail1.SetSenderLevel(500);
            /*
            foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
            {
                var p = new AvatarStreamEntryMessage(onlinePlayer.GetClient());
                var pm = new GlobalChatLineMessage(onlinePlayer.GetClient());
                pm.SetChatMessage("Our current Server Status is now sent at your mailbox!");
                pm.SetPlayerId(0);
                pm.SetLeagueId(22);
                pm.SetPlayerName("System Manager");
                p.SetAvatarStreamEntry(mail1);
                PacketManager.ProcessOutgoingPacket(p);
                PacketManager.ProcessOutgoingPacket(pm);
            }*/
        }
        private void SwitchCommand()
        {
            if (ConfUCS.IsConsoleFirst)
                Console.WriteLine("[UCS]   Sorry, you need to launch UCS in GUI mode first.");
            else
            {
                if (ConfUCS.IsConsoleMode)
                {
                    ConfUCS.IsConsoleMode = false;
                    ConsoleManage.HideConsole();
                    Console.SetOut(MainWindow.CS);
                    InterfaceThread.Start();
                    Console.WriteLine("[UCS]   Switched to GUI");
                    ControlTimer.SwitchTimer();
                }
                else
                {
                    ConfUCS.IsConsoleMode = true;
                    ConsoleManage.ShowConsole();
                    Console.SetOut(AllocateConsole.StandardConsole);
                    MainWindow.RemoteWindow.Hide();
                    Console.Title = ConfUCS.UnivTitle;
                    Console.WriteLine("[UCS]   Switched to Console");
                    ControlTimer.SwitchTimer();
                    CommandParser.ManageConsole();
                }
            }
        }
        private void PluginsCommand()
        {
            Console.WriteLine("\n[UCS]   Number of plugins: {0}", ConfUCS.PM.LoadedPluginsICP.Count + ConfUCS.PM.LoadedPluginsIGP.Count);
            Console.WriteLine("------------------------------------------------");

            for (int i = 0; i < ConfUCS.PM.LoadedPluginsICP.Count; i++)
            {
                Console.WriteLine("|| Name: {0} \nAuthor: {1} \nDescription: {2} \nType: Command Handler Plugin", ConfUCS.PM.LoadedPluginsICP[i].plugin.Title, ConfUCS.PM.LoadedPluginsICP[i].plugin.AuthorName, ConfUCS.PM.LoadedPluginsICP[i].plugin.Information);
                Console.WriteLine("------------------------------------------------");
            }
            for (int i = 0; i < ConfUCS.PM.LoadedPluginsIGP.Count; i++)
            {
                Console.WriteLine("|| Name: {0} \nAuthor: {1} \nDescription: {2} \nType: Generic Plugin", ConfUCS.PM.LoadedPluginsIGP[i].plugin.Title, ConfUCS.PM.LoadedPluginsIGP[i].plugin.AuthorName, ConfUCS.PM.LoadedPluginsIGP[i].plugin.Information);
                Console.WriteLine("------------------------------------------------");
            }
            Console.WriteLine("[UCS]   End of list of plugins\n");
        }

        private void KickCommand()
        {
            var CommGet = CommandParser.GetCommand.Split(' ');
            if (CommGet.Length >= 2)
            {
                try
                {
                    var id = Convert.ToInt64(CommGet[1]);
                    var l = ResourcesManager.GetPlayer(id);
                    if (ResourcesManager.IsPlayerOnline(l))
                    {
                        ResourcesManager.LogPlayerOut(l);
                        var p = new OutOfSyncMessage(l.GetClient());
                        PacketManager.ProcessOutgoingPacket(p);
                    }
                    else
                    {
                        Console.WriteLine("[UCS]   Kick failed: id " + id + " not found");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("[UCS]   The given id is not a valid number");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[UCS]   Kick failed with error: " + ex);
                }
            }
            else Console.WriteLine("[UCS]   Not enough arguments");
        }
        private void BanCommand()
        {
            var CommGet1 = CommandParser.GetCommand.Split(' ');
            if (CommGet1.Length >= 2)
            {
                try
                {
                    var id = Convert.ToInt64(CommGet1[1]);
                    var l = ResourcesManager.GetPlayer(id);
                    if (l != null)
                    {
                        l.SetAccountStatus(99);
                        l.SetAccountPrivileges(0);
                        if (ResourcesManager.IsPlayerOnline(l))
                        {
                            var p = new OutOfSyncMessage(l.GetClient());
                            PacketManager.ProcessOutgoingPacket(p);
                        }
                    }
                    else
                    {
                        Console.WriteLine("[UCS]   Ban failed: id " + id + " not found");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("[UCS]   The given id is not a valid number");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[UCS]   Ban failed with error: " + ex);
                }
            }
            else Console.WriteLine("[UCS]   Not enough arguments");
        }
        private void UnbanCommand()
        {
            var CommGet2 = CommandParser.GetCommand.Split(' ');
            if (CommGet2.Length >= 2)
            {
                try
                {
                    var id = Convert.ToInt64(CommGet2[1]);
                    var l = ResourcesManager.GetPlayer(id);
                    if (l != null)
                    {
                        l.SetAccountStatus(0);
                    }
                    else
                    {
                        Console.WriteLine("[UCS]   Unban failed: id " + id + " not found");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("[UCS]   The given id is not a valid number");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[UCS]   Unban failed with error: " + ex);
                }
            }
            else Console.WriteLine("[UCS]   Not enough arguments");
        }
        private void SayCommand()
        {
            if (CommandParser.GetCommand.Length < 6)
            {
                Console.WriteLine("[UCS]   No text found after /say command.");
                return;
            }

            var str = CommandParser.GetCommand.Substring(5);
            /*
            foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
            {
                var pm = new GlobalChatLineMessage(onlinePlayer.GetClient());
                pm.SetChatMessage(str);
                pm.SetPlayerId(0);
                pm.SetLeagueId(22);
                pm.SetPlayerName("System Manager");
                PacketManager.ProcessOutgoingPacket(pm);
            }*/

            Console.WriteLine("[UCS]<SERVER> {0}", str);
        }
    }
}
