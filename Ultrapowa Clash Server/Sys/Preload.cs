using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace UCS.Sys
{
    class Preload
    {

        List<string> GameListFiles;
        List<string> CoreFiles;
        List<string> MissingNotReqFiles = new List<string>();
        double localvalue = 0;
        double Inc = 0;
        string MonoLine;

        public void PreloadThings()
        {

            if (ConfUCS.IsPreloaded) return;

            InitializeFileList();

            double minicounter = 0;

            if (ConfUCS.IsConsoleMode) Console.Write("Checking gamefiles... ");

            if (!ConfUCS.IsConsoleMode) UI.SplashScreen.SS.Dispatcher.BeginInvoke((Action)delegate ()
            {
                // UI.SplashScreen.SS.label_txt.Content = "Checking files... ";
            });

            //Verify GameFiles
            foreach (string DataG in GameListFiles)
            {

                if (!System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() + @"\" + DataG))
                    MissingNotReqFiles.Add(DataG);

                minicounter++;
                localvalue = (minicounter / GameListFiles.Count);

                if (!ConfUCS.IsConsoleMode) UI.SplashScreen.SS.Dispatcher.BeginInvoke((Action)delegate () {
                    UI.SplashScreen.SS.PB_Loader.Value = Inc + (50 * localvalue);
                });

            }

            minicounter = 0;
            Inc = 50;

            if (!ConfUCS.IsConsoleMode) UI.SplashScreen.SS.Dispatcher.BeginInvoke((Action)delegate () {
                UI.SplashScreen.SS.label_txt.Content = "Verifying required data... ";
            });

            if (MissingNotReqFiles.Count != 0)
            {

                foreach (string FilesMiss in MissingNotReqFiles)
                    MonoLine += FilesMiss + " || ";

                string IsMoreThanOne = MissingNotReqFiles.Count == 1 ? "There is a missing gamefile: " : "There are a missing gamefiles from the directory " + @"""" + System.IO.Directory.GetCurrentDirectory() + @"\ ";
                MessageBox.Show(IsMoreThanOne + MonoLine, "Missing Gamefile", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            if (ConfUCS.IsConsoleMode) Console.Write("OK!\n");
            Console.ResetColor();

            if (ConfUCS.IsConsoleMode) Console.Write("Checking corefiles... ");
            //Verify CoreFiles (DLL, Database, and so on)
            foreach (string DataG in CoreFiles)
            {
                if (!System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() + @"\" + DataG))
                {
                    MessageBox.Show(string.Format("The required file {0} in directory {1} is missing. Cannot continue.",
                        DataG, System.IO.Directory.GetCurrentDirectory()), "Error required file", MessageBoxButton.OK, MessageBoxImage.Error);

                    Thread t = new Thread(() => {
                        UI.SplashScreen.SS.Dispatcher.BeginInvoke((Action)delegate () {
                            Application.Current.Shutdown();
                        });
                    });

                    t.Start(); //Goodbye application :P
                    Assembly.LoadFrom(System.IO.Directory.GetCurrentDirectory() + @"\" + DataG);
                }

                minicounter++;
                localvalue = (minicounter / CoreFiles.Count);

                if (!ConfUCS.IsConsoleMode) UI.SplashScreen.SS.Dispatcher.BeginInvoke((Action)delegate () {
                    UI.SplashScreen.SS.PB_Loader.Value = Inc + (30 * localvalue);
                });
            }

            Console.ForegroundColor = ConsoleColor.Green;
            if (ConfUCS.IsConsoleMode)  Console.Write("OK!\n");
            Console.ResetColor();

            if (!ConfUCS.IsConsoleMode) UI.SplashScreen.SS.Dispatcher.BeginInvoke((Action)delegate ()
            {
                UI.SplashScreen.SS.label_txt.Content = "Loading plugins... ";
                UI.SplashScreen.SS.PB_Loader.Value = 80;
            });

            ConfUCS.PM.PreloadPlugins();

            if (!ConfUCS.IsConsoleMode)
            {
                UI.SplashScreen.SS.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    UI.SplashScreen.SS.label_txt.Content = "Checking update... ";
                    UI.SplashScreen.SS.PB_Loader.Value = 95;
                });

                //UpdateChecker.Check();

                UI.SplashScreen.SS.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    UI.SplashScreen.SS.PB_Loader.Value = 100;
                    UI.SplashScreen.SS.Close();
                });

            }
            ConfUCS.IsPreloaded = true;
        }


        //This is a list of required files. Will check if exist, if not, will show a message and stop.
        //This prevent system crash/corruption datas.
        private void InitializeFileList()
        {
            GameListFiles = new List<string>
            {
                #region csv
                @"Gamefiles\csv\animations.csv",
                @"Gamefiles\csv\billing_packages.csv",
                @"Gamefiles\csv\client_globals.csv",
                @"Gamefiles\csv\credits.csv",
                @"Gamefiles\csv\faq.csv",
                @"Gamefiles\csv\hints.csv",
                @"Gamefiles\csv\news.csv",
                @"Gamefiles\csv\particle_emitters.csv",
                @"Gamefiles\csv\resource_packs.csv",
                @"Gamefiles\csv\texts.csv",
                @"Gamefiles\csv\texts_patch.csv",
                @"Gamefiles\starting_home.json",
                @"Gamefiles\logic\achievements.csv",
                @"Gamefiles\logic\alliance_badge_layers.csv",
                @"Gamefiles\logic\alliance_badges.csv",
                @"Gamefiles\logic\alliance_levels.csv",
                @"Gamefiles\logic\alliance_portal.csv",
                @"Gamefiles\logic\building_classes.csv",
                @"Gamefiles\logic\buildings.csv",
                @"Gamefiles\logic\characters.csv",
                @"Gamefiles\logic\decos.csv",
                @"Gamefiles\logic\effects.csv",
                @"Gamefiles\logic\experience_levels.csv",
                @"Gamefiles\logic\globals.csv",
                @"Gamefiles\logic\heroes.csv",
                @"Gamefiles\logic\leagues.csv",
                @"Gamefiles\logic\locales.csv",
                @"Gamefiles\logic\missions.csv",
                @"Gamefiles\logic\npcs.csv",
                @"Gamefiles\logic\obstacles.csv",
                @"Gamefiles\logic\projectiles.csv",
                @"Gamefiles\logic\regions.csv",
                @"Gamefiles\logic\resources.csv",
                @"Gamefiles\logic\shields.csv",
                @"Gamefiles\logic\resources.csv",
                @"Gamefiles\logic\shields.csv",
                @"Gamefiles\logic\spells.csv",
                @"Gamefiles\logic\townhall_levels.csv",
                @"Gamefiles\logic\traps.csv",
                @"Gamefiles\logic\war.csv",
                @"Gamefiles\logic\variables.csv",
                #endregion

                #region level
                @"Gamefiles\pve\level1.json",
                @"Gamefiles\pve\level2.json",
                @"Gamefiles\pve\level3.json",
                @"Gamefiles\pve\level4.json",
                @"Gamefiles\pve\level5.json",
                @"Gamefiles\pve\level6.json",
                @"Gamefiles\pve\level7.json",
                @"Gamefiles\pve\level8.json",
                @"Gamefiles\pve\level9.json",
                @"Gamefiles\pve\level10.json",
                @"Gamefiles\pve\level11.json",
                @"Gamefiles\pve\level12.json",
                @"Gamefiles\pve\level13.json",
                @"Gamefiles\pve\level14.json",
                @"Gamefiles\pve\level15.json",
                @"Gamefiles\pve\level16.json",
                @"Gamefiles\pve\level17.json",
                @"Gamefiles\pve\level18.json",
                @"Gamefiles\pve\level19.json",
                @"Gamefiles\pve\level20.json",
                @"Gamefiles\pve\level21.json",
                @"Gamefiles\pve\level22.json",
                @"Gamefiles\pve\level23.json",
                @"Gamefiles\pve\level24.json",
                @"Gamefiles\pve\level25.json",
                @"Gamefiles\pve\level26.json",
                @"Gamefiles\pve\level27.json",
                @"Gamefiles\pve\level28.json",
                @"Gamefiles\pve\level29.json",
                @"Gamefiles\pve\level30.json",
                @"Gamefiles\pve\level31.json",
                @"Gamefiles\pve\level32.json",
                @"Gamefiles\pve\level33.json",
                @"Gamefiles\pve\level34.json",
                @"Gamefiles\pve\level35.json",
                @"Gamefiles\pve\level36.json",
                @"Gamefiles\pve\level37.json",
                @"Gamefiles\pve\level38.json",
                @"Gamefiles\pve\level39.json",
                @"Gamefiles\pve\level40.json",
                @"Gamefiles\pve\level41.json",
                @"Gamefiles\pve\level42.json",
                @"Gamefiles\pve\level43.json",
                @"Gamefiles\pve\level44.json",
                @"Gamefiles\pve\level45.json",
                @"Gamefiles\pve\level46.json",
                @"Gamefiles\pve\level47.json",
                @"Gamefiles\pve\level48.json",
                @"Gamefiles\pve\level49.json",
                @"Gamefiles\pve\level50.json"
                #endregion
            };

            CoreFiles = new List<string>
            {
                @"EntityFramework.dll",
                @"EntityFramework.SqlServer.dll",
                @"EntityFramework.SqlServer.dll",
                @"Ionic.Zlib.dll",
                @"MySql.Data.dll",
                @"MySql.Data.Entity.EF6.dll",
                @"Newtonsoft.Json.dll",
                @"Newtonsoft.Json.xml",
                @"System.Data.SQLite.dll",
                @"System.Data.SQLite.EF6.dll",
                @"System.Data.SQLite.Linq.dll",
                @"System.Data.SQLite.xml",
                @"config.ucs",
                @"ucsdb"
            };

        }
    }
}
