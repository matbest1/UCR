using System;
using System.Collections.Generic;
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
    public partial class PlayerInfo : Window
    {
        public List<ConCatPlayers> Players = new List<ConCatPlayers>();
        int DeltaVariation = 100;

        public PlayerInfo()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpInW();
            AnimationLib.MoveToTargetY(btn_ok, DeltaVariation, 0.25, 50);
            AnimationLib.MoveToTargetY(CB_Player, DeltaVariation, 0.25, 100);
            AnimationLib.MoveToTargetY(LB_Main, DeltaVariation, 0.25, 150);
            AnimationLib.MoveToTargetY(img_Commands, DeltaVariation, 0.25, 200);
            AnimationLib.MoveWindowToTargetY(this, DeltaVariation, Top, 0.25);
            UpdatePlayers();
        }


        private void UpdatePlayers()
        {
            Players.Clear();
            lbl_Loading.Visibility = Visibility.Visible;
            PB_Loader.Visibility = Visibility.Visible;
            CB_Player.ItemsSource = null;

            new Thread(() => {
                double count = 0;
                double maxNumber = ResourcesManager.GetAllPlayerIds().Count;
                foreach (var x in ResourcesManager.GetAllPlayerIds())
                {

                    Players.Add(new ConCatPlayers { PlayerIDs = ResourcesManager.GetPlayer(x).GetPlayerAvatar().GetId().ToString(), PlayerNames = ResourcesManager.GetPlayer(x).GetPlayerAvatar().GetAvatarName().ToString() });
                    Dispatcher.BeginInvoke((Action)delegate {
                        PB_Loader.Value = (count / maxNumber) * 100D;
                    });
                    count++;
                }

                Dispatcher.BeginInvoke((Action)delegate
                {
                    CB_Player.ItemsSource = Players;
                    lbl_Loading.Visibility = Visibility.Hidden;
                    PB_Loader.Visibility = Visibility.Hidden;
                });
            }).Start();
        }

        private void OpInW()
        {
            var OpIn = new DoubleAnimation(1, TimeSpan.FromSeconds(0.125));
            BeginAnimation(OpacityProperty, OpIn);
        }

        private void OpOutW(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Closing -= Window_Closing;
            e.Cancel = true;
            var OpOut = new DoubleAnimation(0, TimeSpan.FromSeconds(0.125));
            OpOut.Completed += (s, _) => { this.Close(); MainWindow.IsFocusOk = true; };
            BeginAnimation(OpacityProperty, OpOut);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => OpOutW(sender, e);

        private void btn_ok_Click(object sender, RoutedEventArgs e) => Close();
    }
}
