using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using UCS.Core;
using UCS.Helpers;

namespace UCS.UI
{
    /// <summary>
    /// Logica di interazione per Popup.xaml
    /// </summary>
    public partial class Popup : Window
    {

        bool IsRequiredSecPage = false;
        bool WasSecPage = false;
        int DeltaVariation = 100;
        bool IsErrorHappens = false;

        public class cause
        {
            public const int BAN = 0;
            public const int BANIP = 1;
            public const int TEMPBAN = 2;
            public const int TEMPBANIP = 3;
            public const int UNBAN = 4;
            public const int UNBANIP = 5;
            public const int MUTE = 6;
            public const int UNMUTE = 7;
            public const int KICK = 8;
        }

        public int CC = -1;

        public Popup(int Slc_cause = -1)
        {
            Opacity = 0;

            InitializeComponent();


            LB_Main.Content = Slc_cause == cause.BAN ? "Select a player to ban" : Slc_cause == cause.BANIP ?
                "Select a player to ban ip" : Slc_cause == cause.TEMPBAN ? "Select a player to ban temporarily" :
                Slc_cause == cause.TEMPBANIP ? "Select a player to ban ip" : Slc_cause == cause.UNBAN ?
                "Select a player to unban" : Slc_cause == cause.UNBANIP ? "Select a player to unban ip" :
                Slc_cause == cause.MUTE ? "Select a player to mute" : Slc_cause == cause.UNMUTE ? 
                "Select a player to unmute" : Slc_cause == cause.KICK ? "Select a player to kick" : 
                "Error";

            if (Slc_cause == cause.UNBAN || Slc_cause == cause.UNBANIP || Slc_cause == cause.MUTE ||
                Slc_cause == cause.UNMUTE || Slc_cause == cause.KICK || Slc_cause == cause.BAN)
            {

                btn_ok.Content = Properties.Resources.OK;
                IsRequiredSecPage = false;

            }
            else if (Slc_cause == -1)
            {
                btn_ok.Content = Properties.Resources.Exit;
                IsErrorHappens = false;
                IsRequiredSecPage = false;
            }
            else
            {
                btn_ok.Content = Properties.Resources.Continue;
                IsRequiredSecPage = true;
            }

            CC = Slc_cause;

        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpInW();

            MainWindow.RemoteWindow.UpdateTheListPlayers();
            CB_Player.ItemsSource = MainWindow.RemoteWindow.Players;

            AnimationLib.MoveToTargetY(btn_cancel, DeltaVariation, 0.25);
            AnimationLib.MoveToTargetY(btn_ok, DeltaVariation, 0.25, 50);
            AnimationLib.MoveToTargetY(CB_Player, DeltaVariation, 0.25, 100);
            AnimationLib.MoveToTargetY(LB_Main, DeltaVariation, 0.25, 150);
            AnimationLib.MoveToTargetY(img_Commands, DeltaVariation, 0.25, 200);

            AnimationLib.MoveWindowToTargetY(this, DeltaVariation, Top, 0.25);

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => OpOutW(sender, e);

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

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            if (IsErrorHappens)
            {
                Close();
                return;
            }

            if (!IsRequiredSecPage)
            {
                if (CB_Player.SelectedIndex == -1)
                    MessageBox.Show(Properties.Resources.SelectAPlayerFirst);
                else {
                    string[] SPLT = CB_Player.SelectedItem.ToString().Split(' ');
                    switch (CC)
                    {
                        case 0:
                            CommandParser.CommandRead("/ban " + SPLT[2]);
                            Close(); break;
                        case 4:
                            CommandParser.CommandRead("/unban " + SPLT[2]);
                            Close(); break;
                        case 8:
                            CommandParser.CommandRead("/kick " + SPLT[2]);
                            Close(); break;
                    }

                }
            }
        }
    }
}
