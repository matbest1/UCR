using System;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml;

namespace UCS.UI
{
    /// <summary>
    /// Logica di interazione per GeneralPopup.xaml
    /// </summary>
    public partial class PopupConfiguration : Window
    {
        public PopupConfiguration()
        {
            Opacity = 0;
            InitializeComponent();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadConfig();
            OpInW();

            int DeltaVariation = 300;
            AnimationLib.MoveToTargetY(CB_EnableMaintenance, -DeltaVariation/2, 0.25);
            AnimationLib.MoveToTargetY(lbl_EnableMaintenance, -DeltaVariation/2, 0.25, 50);
            AnimationLib.MoveToTargetY(BTN_Load, -DeltaVariation/2, 0.25, 100);
            AnimationLib.MoveToTargetY(BTN_Save, -DeltaVariation/2, 0.25, 150);
            AnimationLib.MoveToTargetY(BTN_Discard, -DeltaVariation / 2, 0.25, 200);
            AnimationLib.MoveToTargetX(TB_Gems, DeltaVariation, 0.25);
            AnimationLib.MoveToTargetX(lbl_Gems, DeltaVariation, 0.25, 25);
            AnimationLib.MoveToTargetX(TB_Gold, DeltaVariation, 0.25, 25);
            AnimationLib.MoveToTargetX(lbl_Gold, DeltaVariation, 0.25, 50);
            AnimationLib.MoveToTargetX(TB_Elixir, DeltaVariation, 0.25, 50);
            AnimationLib.MoveToTargetX(lbl_Elixir, DeltaVariation, 0.25, 75);
            AnimationLib.MoveToTargetX(TB_DarkElixir, DeltaVariation, 0.25, 75);
            AnimationLib.MoveToTargetX(lbl_DarkElixir, DeltaVariation, 0.25, 100);
            AnimationLib.MoveToTargetX(TB_Trophies, DeltaVariation, 0.25, 100);
            AnimationLib.MoveToTargetX(lbl_Trophies, DeltaVariation, 0.25, 125);
            AnimationLib.MoveToTargetX(TB_Shield, DeltaVariation, 0.25, 125);
            AnimationLib.MoveToTargetX(lbl_Shield, DeltaVariation, 0.25, 150);
            AnimationLib.MoveToTargetX(TB_StartingLevel, DeltaVariation, 0.25, 150);
            AnimationLib.MoveToTargetX(lbl_StartingLevel, DeltaVariation, 0.25, 175);
            AnimationLib.MoveToTargetX(TB_Experience, DeltaVariation, 0.25, 175);
            AnimationLib.MoveToTargetX(lbl_Experience, DeltaVariation, 0.25, 200);

            AnimationLib.MoveToTargetX(lbl_PatchServer, -DeltaVariation, 0.25);
            AnimationLib.MoveToTargetX(TB_PatchServer, -DeltaVariation, 0.25, 25);
            AnimationLib.MoveToTargetX(lbl_Outdated, -DeltaVariation, 0.25, 25);
            AnimationLib.MoveToTargetX(TB_Outdated, -DeltaVariation, 0.25, 50);
            AnimationLib.MoveToTargetX(lbl_ConnName, -DeltaVariation, 0.25, 50);
            AnimationLib.MoveToTargetX(CB_ConnName, -DeltaVariation, 0.25, 75);
            AnimationLib.MoveToTargetX(lbl_ClientVer, -DeltaVariation, 0.25, 75);
            AnimationLib.MoveToTargetX(TB_ClientVer, -DeltaVariation, 0.25, 100);
            AnimationLib.MoveToTargetX(lbl_Maintenance, -DeltaVariation, 0.25, 100);
            AnimationLib.MoveToTargetX(TB_Maintenance, -DeltaVariation, 0.25, 125);
            AnimationLib.MoveToTargetX(lbl_Port, -DeltaVariation, 0.25, 125);
            AnimationLib.MoveToTargetX(TB_Port, -DeltaVariation, 0.25, 150);
            AnimationLib.MoveToTargetX(lbl_DebugPort, -DeltaVariation, 0.25, 150);
            AnimationLib.MoveToTargetX(TB_DebugPort, -DeltaVariation, 0.25, 175);
            AnimationLib.MoveToTargetX(lbl_EnableDebug, -DeltaVariation, 0.25, 175);
            AnimationLib.MoveToTargetX(CB_EnableDebug,-DeltaVariation, 0.25, 200);
            AnimationLib.MoveToTargetX(lbl_LogLevel, -DeltaVariation, 0.25, 200);
            AnimationLib.MoveToTargetX(TB_LogLevel, -DeltaVariation, 0.25, 225);
            AnimationLib.MoveToTargetX(lbl_CustomPatch, -DeltaVariation, 0.25, 225);
            AnimationLib.MoveToTargetX(CB_CustomPatch, -DeltaVariation, 0.25, 250);
            AnimationLib.MoveToTargetX(lbl_APIManager, -DeltaVariation, 0.25, 250);
            AnimationLib.MoveToTargetX(CB_APIManager, -DeltaVariation, 0.25, 275);

            AnimationLib.MoveToTargetY(lbl_Title, DeltaVariation, 0.25, 150);
            AnimationLib.MoveToTargetY(img_Utility, DeltaVariation, 0.25, 200);

            AnimationLib.MoveWindowToTargetY(this, 100, Top, 0.25);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OpOutW(sender, e);
        }

        private void OpInW()
        {
            var OpIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.25));
            BeginAnimation(OpacityProperty, OpIn);
        }

        private void OpOutW(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Closing -= Window_Closing;
            e.Cancel = true;
            var OpOut = new DoubleAnimation(0, TimeSpan.FromSeconds(0.25));
            OpOut.Completed += (s, _) => { Close(); MainWindow.IsFocusOk = true; };
            BeginAnimation(OpacityProperty, OpOut);
        }

        private void BTN_SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in BlockSaves)
                if (item)
                { 
                    MessageBox.Show("There are some invalid values, fix it before saving.\nHelp: Starting items should not exceed the maximum value of 999999999 and the minum value of 0\nPort max value: 65535 and should be same of debug port\nStarting level max value: 9");
                    return;
                }

            SaveChanges();
        }

        private void BTN_ReloadConfig_Click(object sender, RoutedEventArgs e)
        {
            LoadConfig();
        }

        private void BTN_DiscardChanges_Click(object sender, RoutedEventArgs e)
        {
            var DG = MessageBox.Show("Are you sure?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (DG == MessageBoxResult.Yes)
                Close();
        }

        private void SaveChanges()
        {
            var doc = new XmlDocument();
            var path = "config.ucs";
            doc.Load(path);
            var ie = doc.SelectNodes("appSettings/add").GetEnumerator();

            while (ie.MoveNext())
            {
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingDarkElixir") (ie.Current as XmlNode).Attributes["value"].Value = TB_DarkElixir.Text;
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingElixir") (ie.Current as XmlNode).Attributes["value"].Value = TB_Elixir.Text;
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingGold") (ie.Current as XmlNode).Attributes["value"].Value = TB_Gold.Text;
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingGems") (ie.Current as XmlNode).Attributes["value"].Value = TB_Gems.Text;
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingLevel") (ie.Current as XmlNode).Attributes["value"].Value = TB_StartingLevel.Text;
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingTrophies") (ie.Current as XmlNode).Attributes["value"].Value = TB_Trophies.Text;
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingExperience") (ie.Current as XmlNode).Attributes["value"].Value = TB_Experience.Text;
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingShieldTime") (ie.Current as XmlNode).Attributes["value"].Value = TB_Shield.Text;
                if ((ie.Current as XmlNode).Attributes["key"].Value == "clientVersion") (ie.Current as XmlNode).Attributes["value"].Value = TB_ClientVer.Text;
                if ((ie.Current as XmlNode).Attributes["key"].Value == "patchingServer") (ie.Current as XmlNode).Attributes["value"].Value = TB_PatchServer.Text;
                if ((ie.Current as XmlNode).Attributes["key"].Value == "maintenanceTimeleft") (ie.Current as XmlNode).Attributes["value"].Value = TB_Maintenance.Text;
                if ((ie.Current as XmlNode).Attributes["key"].Value == "loggingLevel") (ie.Current as XmlNode).Attributes["value"].Value = TB_LogLevel.Text;
                if ((ie.Current as XmlNode).Attributes["key"].Value == "oldClientVersion") (ie.Current as XmlNode).Attributes["value"].Value = TB_Outdated.Text;
                if ((ie.Current as XmlNode).Attributes["key"].Value == "proDebugPort") (ie.Current as XmlNode).Attributes["value"].Value = TB_DebugPort.Text;
                   
                if ((ie.Current as XmlNode).Attributes["key"].Value == "databaseConnectionName") {
                    var cont = CN_T.IsSelected ? "sqliteEntities" : "ucsdbEntities";
                    (ie.Current as XmlNode).Attributes["value"].Value = cont;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "useCustomPatch") {
                    var cont = CP_T.IsSelected ? "True" : "False";
                    (ie.Current as XmlNode).Attributes["value"].Value = cont;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "apiManager") {
                    var cont = AM_T.IsSelected ? "True" : "False";
                    (ie.Current as XmlNode).Attributes["value"].Value = cont;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "debugMode") {
                    var cont = ED_T.IsSelected ? "True" : "False";
                    (ie.Current as XmlNode).Attributes["value"].Value = cont;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "maintenanceMode") {
                    var cont = EM_T.IsSelected ? "True" : "False";
                    (ie.Current as XmlNode).Attributes["value"].Value = cont;
                }
            }

            doc.Save(path);
            MessageBox.Show("Saved!", "Configuration saved", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            ConfigurationManager.RefreshSection("appSettings");
            Close();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void LoadConfig()
        {

            //Need to fix reloading config after saving. No problems found on saving.
            NeedToCheck = false;
            TB_DarkElixir.Text = ConfigurationManager.AppSettings["startingDarkElixir"];
            TB_Elixir.Text = ConfigurationManager.AppSettings["startingElixir"];
            TB_Gold.Text = ConfigurationManager.AppSettings["startingGold"];
            TB_Gems.Text = ConfigurationManager.AppSettings["startingGems"];
            TB_StartingLevel.Text = ConfigurationManager.AppSettings["startingLevel"];
            TB_Trophies.Text = ConfigurationManager.AppSettings["startingTrophies"];
            TB_Experience.Text = ConfigurationManager.AppSettings["startingExperience"];
            TB_Shield.Text = ConfigurationManager.AppSettings["startingShieldTime"];
            TB_ClientVer.Text = ConfigurationManager.AppSettings["clientVersion"];
            TB_PatchServer.Text = ConfigurationManager.AppSettings["patchingServer"];
            TB_Maintenance.Text = ConfigurationManager.AppSettings["maintenanceTimeleft"];
            TB_LogLevel.Text = ConfigurationManager.AppSettings["loggingLevel"];
            TB_Outdated.Text = ConfigurationManager.AppSettings["oldClientVersion"];
            TB_DebugPort.Text = ConfigurationManager.AppSettings["proDebugPort"];
            TB_Port.Text = "9339";

            var CN = ConfigurationManager.AppSettings["databaseConnectionName"];
            if (CN.ToLower() == "sqliteentities") { CN_T.IsSelected = true; CN_F.IsSelected = false; }
            else { CN_F.IsSelected = true; CN_T.IsSelected = false; }

            var CP = Convert.ToString(Convert.ToBoolean(ConfigurationManager.AppSettings["useCustomPatch"]));
            if (CP.ToLower() == "true") { CP_T.IsSelected = true; CP_F.IsSelected = false; }
            else { CP_F.IsSelected = true; CP_T.IsSelected = false; }

            var AM = Convert.ToString(Convert.ToBoolean(ConfigurationManager.AppSettings["apiManager"]));
            if (AM.ToLower() == "true") { AM_T.IsSelected = true; AM_F.IsSelected = false; }
            else { AM_F.IsSelected = true; AM_F.IsSelected = false; }

            var ED = Convert.ToString(Convert.ToBoolean(ConfigurationManager.AppSettings["debugMode"]));
            if (ED.ToLower() == "true") { ED_T.IsSelected = true; ED_F.IsSelected = false; }
            else { ED_F.IsSelected = true; ED_T.IsSelected = false; }

            var EM = Convert.ToString(Convert.ToBoolean(ConfigurationManager.AppSettings["maintenanceMode"]));
            if (EM.ToLower() == "true") { EM_T.IsSelected = true; EM_F.IsSelected = false; }
            else { EM_F.IsSelected = true; EM_T.IsSelected = false; }

            //var PH = ConfigurationManager.AppSettings["expertPve"];
            //if (PH.ToLower() == "true") { PH_T.IsSelected = true; PH_F.IsSelected = false; }
            //else { PH_F.IsSelected = true; PH_T.IsSelected = false; }

            NeedToCheck = true;
        }

        public bool[] BlockSaves = new bool[10];

        public bool NeedToCheck = false;
        

        private bool CheckValues(string X, int maxmax = 999999999, int minmin = 0)
        {
            int WOW;
            try
            {
                WOW = Convert.ToInt32(X);
            }
            catch (Exception)
            {
                return false;
            }
            if (WOW > maxmax || WOW < minmin) return false;

            return true;
        }

        SolidColorBrush GOOD = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x69, 0x7C)); //0
        SolidColorBrush ERR = new SolidColorBrush(Color.FromArgb(0xFF, 0xB4, 0x2A, 0x0C)); //2

        #region EVENTS
        private void TB_Gems_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool IsOk = CheckValues(TB_Gems.Text);
            if (IsOk)
            {
                TB_Gems.Background = GOOD;
                BlockSaves[0] = false;
            }
            else
            {
                TB_Gems.Background = ERR;
                BlockSaves[0] = true;
            }
        }

        private void TB_Port_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NeedToCheck)
            {
                bool IsOk = CheckValues(TB_Port.Text, 65535);
                if (IsOk)
                {
                    TB_Port.Background = GOOD;
                    BlockSaves[8] = false;

                    int Con1 = 0, Con2 = 0;
                    try
                    {
                        Con1 = Convert.ToInt32(TB_DebugPort.Text);
                    }
                    catch (Exception) { }
                    try
                    {
                        Con2 = Convert.ToInt32(TB_Port.Text);
                    }
                    catch (Exception) { }

                    if (Con1 == Con2)
                    {
                        TB_Port.Background = ERR;
                        TB_DebugPort.Background = ERR;
                        BlockSaves[8] = true;
                        BlockSaves[9] = true;
                    }
                    else
                    {
                        bool IsOk1 = CheckValues(TB_DebugPort.Text, 65535);
                        if (IsOk1)
                        {
                            TB_DebugPort.Background = GOOD;
                            BlockSaves[9] = false;
                        }
                    }
                }
                else
                {
                    TB_Port.Background = ERR;
                    BlockSaves[8] = true;
                }
            }
        }

        private void TB_DebugPort_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NeedToCheck)
            {
                bool IsOk = CheckValues(TB_DebugPort.Text, 65535);
                if (IsOk)
                {
                    TB_DebugPort.Background = GOOD;
                    BlockSaves[9] = false;
                    int Con1=0, Con2=0;
                    try
                    {
                        Con1 = Convert.ToInt32(TB_DebugPort.Text);
                    }
                    catch (Exception) { }
                    try
                    {
                        Con2 = Convert.ToInt32(TB_Port.Text);
                    }
                    catch (Exception) { }

                    if (Con1 == Con2)
                    {
                        TB_Port.Background = ERR;
                        TB_DebugPort.Background = ERR;
                        BlockSaves[8] = true;
                        BlockSaves[9] = true;
                    }
                    else
                    {
                        bool IsOk1 = CheckValues(TB_Port.Text, 65535);
                        if (IsOk1)
                        {
                            TB_Port.Background = GOOD;
                            BlockSaves[8] = false;
                        }
                    }
                }
                else
                {
                    TB_DebugPort.Background = ERR;
                    BlockSaves[9] = true;
                }
            }
        }

        private void TB_Gold_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool IsOk = CheckValues(TB_Gold.Text);
            if (IsOk)
            {
                TB_Gold.Background = GOOD;
                BlockSaves[1] = false;
            }
            else
            {
                TB_Gold.Background = ERR;
                BlockSaves[1] = true;
            }
        }
        private void TB_Elixir_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool IsOk = CheckValues(TB_Elixir.Text);
            if (IsOk)
            {
                TB_Elixir.Background = GOOD;
                BlockSaves[2] = false;
            }
            else
            {
                TB_Elixir.Background = ERR;
                BlockSaves[2] = true;
            }
        }
        private void TB_Trophies_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool IsOk = CheckValues(TB_Trophies.Text,9999,0);
            if (IsOk)
            {
                TB_Trophies.Background = GOOD;
                BlockSaves[4] = false;
            }
            else
            {
                TB_Trophies.Background = ERR;
                BlockSaves[4] = true;
            }
        }

        private void TB_Experience_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool IsOk = CheckValues(TB_Experience.Text,100);
            if (IsOk)
            {
                TB_Experience.Background = GOOD;
                BlockSaves[5] = false;
            }
            else
            {
                TB_Experience.Background = ERR;
                BlockSaves[5] = true;
            }
        }

        private void TB_Shield_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool IsOk = CheckValues(TB_Shield.Text, 2147483647);
            if (IsOk)
            {
                TB_Shield.Background = GOOD;
                BlockSaves[6] = false;
            }
            else
            {
                TB_Shield.Background = ERR;
                BlockSaves[6] = true;
            }
        }

        private void TB_StartingLevel_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool IsOk = CheckValues(TB_StartingLevel.Text,9);
            if (IsOk)
            {
                TB_StartingLevel.Background = GOOD;
                BlockSaves[7] = false;
            }
            else
            {
                TB_StartingLevel.Background = ERR;
                BlockSaves[7] = true;
            }
        }

        private void TB_DarkElixir_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool IsOk = CheckValues(TB_DarkElixir.Text);
            if (IsOk)
            {
                TB_DarkElixir.Background = GOOD;
                BlockSaves[3] = false;
            }
            else
            {
                TB_DarkElixir.Background = ERR;
                BlockSaves[3] = true;
            }
        }
        #endregion
    }
}
