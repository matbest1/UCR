using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Threading.Tasks;

using UCS.Core;
using UCS.Sys;
using UCS.Helpers;
using UCS.Core.Threading;
using UCS.UI;

namespace UCS
{

    public partial class MainWindow : Window
    {
        public static MainWindow RemoteWindow = new MainWindow();
        public static bool IsFocusOk = true;
        public DispatcherTimer UpdateInfoGUI = new DispatcherTimer();
        bool ChangeUpdatePopup = false;
        public static ConsoleStreamer CS;
        public const int port = 9339;
        public List<string> CommandList = new List<string>();

        public bool isBlurEnabled = false;
        public const int blurValue = 10;
        public const RenderingBias RenderQuality = RenderingBias.Performance;

        public MainWindow()
        {
            InitializeComponent();
            RemoteWindow = this;
            ConfUCS.OnServerOnlineEvent += ConfUCS_OnServerOnlineEvent;

            SharedShow();
            label_player.Content = Properties.Resources.PlayersOnline + ": " + "(0)";

            Console.WriteLine(Properties.Resources.LoadingGUI);
            LBL_IP.Content = Properties.Resources.LocalIP + " " + ConfUCS.GetIP() + ":" + port;
            CommandLine.TextChanged += new TextChangedEventHandler(CommandLine_TextChanged);
            
        }

        public void SharedShow()
        {
            CS = new ConsoleStreamer(RTB_Console);
            Console.SetOut(CS);
            Title = ConfUCS.UnivTitle;
        }

        public void PrepareTimer()
        {
            UpdateInfoGUI.Tick += UpdateInfo_Tick;
            UpdateInfoGUI.Interval = new TimeSpan(10000);
        }

        private void UpdateInfo_Tick(object sender, EventArgs e) => ControlTimer.UpdateTime();

        private void ConfUCS_OnServerOnlineEvent(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke((Action)delegate()
            {
                SolidColorBrush m_Color;

                if (ConfUCS._IsServerOnline)
                {
                    m_Color = new SolidColorBrush(Color.FromRgb(0x39, 0xb5, 0x4a));
                    BTN_LaunchServerText.Text = Properties.Resources.ServerOnline;
                    BTN_LaunchServerImage.Source = new BitmapImage(new Uri("/UI/Images/Ok.png", UriKind.Relative));
                    BTN_LaunchServer.IsEnabled = true;
                }
                else
                {
                    m_Color = new SolidColorBrush(Color.FromRgb(0x00, 0x77, 0x9f));
                    BTN_LaunchServerText.Text = Properties.Resources.LaunchServer;
                    BTN_LaunchServerImage.Source = new BitmapImage(new Uri("/UI/Images/Launch.png", UriKind.Relative));
                    BTN_LaunchServer.IsEnabled = true;
                }

                BTN_LaunchServer.Foreground = m_Color;
                (BTN_LaunchServer.Template.FindName("border", BTN_LaunchServer) as Border).BorderBrush = m_Color;
            });
        }

        #region UC Events
        private void UC_Commands_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_Commands.Opacity == 1)
            {
                AnimationLib.OpacityandHeightAnimation(Grid_Commands, 0, 0, 0.25);
                UC_Commands.IsPressed = false;
            }
            else
            {
                AnimationLib.OpacityandHeightAnimation(Grid_Commands, 1, 108, 0.25);
                AnimationLib.OpacityandHeightAnimation(Grid_Utility, 0, 0, 0.25);
                AnimationLib.OpacityandHeightAnimation(Grid_Menu, 0, 0, 0.25);
                UC_Utility.IsPressed = false;
                UC_Commands.IsPressed = true;
            }
        }

        private void UC_Utility_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_Utility.Opacity == 1)
            {
                AnimationLib.OpacityandHeightAnimation(Grid_Utility, 0, 0, 0.25);
                UC_Utility.IsPressed = false;
            }
            else
            {
                AnimationLib.OpacityandHeightAnimation(Grid_Commands, 0, 0, 0.25);
                AnimationLib.OpacityandHeightAnimation(Grid_Menu, 0, 0, 0.25);
                AnimationLib.OpacityandHeightAnimation(Grid_Utility, 1, 108, 0.25);
                UC_Commands.IsPressed = false;
                UC_Utility.IsPressed = true;
            }
        }

        private void UC_Menu_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_Menu.Opacity == 1)
            {
                AnimationLib.OpacityandHeightAnimation(Grid_Menu, 0, 0, 0.25);
            }
            else
            {
                AnimationLib.OpacityandHeightAnimation(Grid_Commands, 0, 0, 0.25);
                AnimationLib.OpacityandHeightAnimation(Grid_Menu, 1, 108, 0.25);
                AnimationLib.OpacityandHeightAnimation(Grid_Utility, 0, 0, 0.25);
                UC_Utility.IsPressed = false;
                UC_Commands.IsPressed = false;
            }
        }

        private void UC_Restart_Click(object sender, RoutedEventArgs e) => CommandParser.CommandRead("/restart");

        private void UC_Exit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void UC_Configuration_Click(object sender, RoutedEventArgs e)
        {
            AnimationLib.OpacityandHeightAnimation(Grid_Utility, 0, 0, 0.25);
            UC_Utility.IsPressed = false;
            IsFocusOk = false;
            PopupConfiguration PC = new PopupConfiguration();
            PC.Owner = this;
            PC.ShowDialog();
        }

        private void UC_Ban_Click(object sender, RoutedEventArgs e)
        {
            AnimationLib.OpacityandHeightAnimation(Grid_Commands, 0, 0, 0.25);
            UC_Commands.IsPressed = false;
            SendPopup(Popup.cause.BAN);
        }

        private void UC_Unban_Click(object sender, RoutedEventArgs e)
        {
            AnimationLib.OpacityandHeightAnimation(Grid_Commands, 0, 0, 0.25);
            UC_Commands.IsPressed = false;
            SendPopup(Popup.cause.UNBAN);
        }

        private void UC_Kick_Click(object sender, RoutedEventArgs e)
        {
            AnimationLib.OpacityandHeightAnimation(Grid_Commands, 0, 0, 0.25);
            UC_Commands.IsPressed = false;
            SendPopup(Popup.cause.KICK);
        }

        private void UC_CheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            AnimationLib.OpacityandHeightAnimation(Grid_Utility, 0, 0, 0.25);
            UC_Utility.IsPressed = false;
            if (!ChangeUpdatePopup)
            {
                IsFocusOk = false;
                PopupUpdater PopupUpdater = new PopupUpdater();
                PopupUpdater.Owner = this;
                PopupUpdater.ShowDialog();
            }
        }

        private void UC_Plugins_Click(object sender, RoutedEventArgs e)
        {
            AnimationLib.OpacityandHeightAnimation(Grid_Commands, 0, 0, 0.25);
            AnimationLib.OpacityandHeightAnimation(Grid_Menu, 0, 0, 0.25);
            AnimationLib.OpacityandHeightAnimation(Grid_Utility, 0, 0, 0.25);
            UC_Commands.IsPressed = false;
            UC_Utility.IsPressed = false;

            IsFocusOk = false;
            var Popup = new PopupPluginManager();
            Popup.Owner = this;
            Popup.ShowDialog();

        }

        private void UC_MouseEnter(object sender, MouseEventArgs e) => AnimationLib.ChangeBackgroundColor((sender as UI.UC.Menu), Color.FromRgb(0x00, 0x4c, 0x65), 0.2);

        private void UC_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!(sender as UI.UC.Menu).IsPressed)
                AnimationLib.ChangeBackgroundColor((sender as UI.UC.Menu), Color.FromRgb(0x00, 0x77, 0x9F), 0.2);
        }

        //Events for SubCommands
        private void USC_MouseLeave(object sender, MouseEventArgs e)
        {
            AnimationLib.ChangeBackgroundColor((sender as UI.UC.Menu), Color.FromRgb(0x00, 0x77, 0x9F), 0.2);
            AnimationLib.MoveToTargetXwoMargin((sender as Control), 0, 10, .2);
        }

        private void USC_MouseEnter(object sender, MouseEventArgs e)
        {
            AnimationLib.ChangeBackgroundColor((sender as UI.UC.Menu), Color.FromRgb(0x00, 0x4c, 0x65), 0.2);
            AnimationLib.MoveToTargetXwoMargin((sender as Control), 10, 0, .2);
        }

        private void UC_Menu_MouseEnter(object sender, MouseEventArgs e) => AnimationLib.ChangeBackgroundBorderColor(UC_Menu_Background, Color.FromRgb(0x00, 0x4c, 0x65), 0.2);

        private void UC_Menu_MouseLeave(object sender, MouseEventArgs e) => AnimationLib.ChangeBackgroundBorderColor(UC_Menu_Background, Color.FromRgb(0x00, 0x77, 0x9F), 0.2);

        #endregion

        #region Events

        private void CommandLine_TextChanged(object sender, TextChangedEventArgs e)
        {
            string TypedCommand = CommandLine.Text;
            List<string> Sug_List = new List<string>();
            Sug_List.Clear();

            foreach (string CM in CommandList)
                if (!string.IsNullOrEmpty(CommandLine.Text))
                    if (CM.StartsWith(TypedCommand))
                        Sug_List.Add(CM);
            
            if (Sug_List.Count > 0)
            {
                LB_CommandTypedList.ItemsSource = Sug_List;
                LB_CommandTypedList.Visibility = Visibility.Visible;
            }
            else if (Sug_List.Count > 0)
            {
                LB_CommandTypedList.ItemsSource = null;
                LB_CommandTypedList.Visibility = Visibility.Collapsed;
            }
            else
            {
                LB_CommandTypedList.ItemsSource = null;
                LB_CommandTypedList.Visibility = Visibility.Collapsed;
            }
        }

        private void LB_CommandTypedList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(LB_CommandTypedList.ItemsSource != null)
            {
                LB_CommandTypedList.Visibility = Visibility.Collapsed;
                CommandLine.TextChanged -= new TextChangedEventHandler(CommandLine_TextChanged);

                if (LB_CommandTypedList.SelectedIndex != -1)
                    CommandLine.Text = LB_CommandTypedList.SelectedItem.ToString();

                CommandLine.TextChanged += new TextChangedEventHandler(CommandLine_TextChanged);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < ConfUCS.PM.LoadedPluginsICP.Count; i++)
                for (int j = 0; j < ConfUCS.PM.LoadedPluginsICP[i].plugin.CommandList().Count; j++)
                    CommandList.Add(ConfUCS.PM.LoadedPluginsICP[i].plugin.CommandList()[j].Command);

            CommandList.Sort();
            Console.WriteLine(Properties.Resources.GUILoaded);
            DoAnimation();

            if (ConfUCS.AutoStartServer)
                if (!ConfUCS.IsServerOnline)
                {
                    BTN_LaunchServer.IsEnabled = false;
                    AsyncUtils.DelayCall(1000, () =>
                    {
                        LaunchConsoleThread();
                    });
                }
        }

        private void LaunchConsoleThread()
        {
            ConsoleThread CT = new ConsoleThread();
            CT.Start();
            BTN_LaunchServer.IsEnabled = false;
        }

        private void BTN_LaunchServer_Click(object sender, RoutedEventArgs e)
        {
            if (!ConfUCS.IsServerOnline)
                LaunchConsoleThread();
            else
                Console.WriteLine(Properties.Resources.ServerAlreadyOnline);
        }

        private void BTN_Enter_Click(object sender, RoutedEventArgs e) => SenderCommand();

        private void CommandLine_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SenderCommand();
        }

        private void SenderCommand()
        {
            if (!string.IsNullOrWhiteSpace(CommandLine.Text))
            {
                CommandParser.CommandRead(CommandLine.Text);
                CommandLine.Clear();
            }
        }
        private void Window_Activated(object sender, EventArgs e)
        {
            if (!IsFocusOk)
                DeBlur(); //Remove the Blur effect
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if (!IsFocusOk)
                DoBlur(); //Start doing the Blur effect
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => Application.Current.Shutdown();

        private void CB_Debug_Unchecked(object sender, RoutedEventArgs e) => ConfUCS.DebugMode = false;

        private void CB_Debug_Checked(object sender, RoutedEventArgs e) => ConfUCS.DebugMode = true;

        #endregion

        #region Do stuff

        public List<ConCatPlayers> Players = new List<ConCatPlayers>();

        public void UpdateTheListPlayers()
        {
            if (!ConfUCS.IsConsoleMode)
            {
                Players.Clear();
                var count = 0;
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    listBox.ItemsSource = null;
                });

                foreach (var x in ResourcesManager.GetOnlinePlayers())
                {
                    count++;
                    Players.Add(new ConCatPlayers { PlayerIDs = x.GetPlayerAvatar().GetId().ToString(), PlayerNames = x.GetPlayerAvatar().GetAvatarName().ToString() });
                }

            
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    listBox.ItemsSource = Players;
                    label_player.Content = Properties.Resources.PlayersOnline + ": " + "(" + count + ")";
                });
            }
        }

        private void SendPopup(int why)
        {
            if (!ConfUCS.IsServerOnline)
                Console.WriteLine(Properties.Resources.ServerNotRunning);
            else
            {
                IsFocusOk = false;
                Popup Popup = new Popup(why);
                Popup.Owner = this;
                Popup.ShowDialog();
            }
        }

        #region Animations
        private void DoAnimation()
        {
            //AYY LMAO

            int DeltaVariation = -100;
            AnimationLib.MoveToTargetY(CB_Debug, DeltaVariation, 0.25,50);
            AnimationLib.MoveToTargetY(LBL_UpTime, DeltaVariation, 0.25,100);
            AnimationLib.MoveToTargetY(img_Timer, DeltaVariation, 0.25, 110);
            AnimationLib.MoveToTargetY(LBL_IP, DeltaVariation, 0.25, 200);
            AnimationLib.MoveToTargetY(img_Internet, DeltaVariation, 0.25, 210);
            AnimationLib.MoveToTargetY(MainRectangle, -DeltaVariation, 0.25, 250);
            AnimationLib.MoveToTargetY(UC_Menu, -DeltaVariation, 0.25, 300);
            AnimationLib.MoveToTargetY(UC_Menu_Background, -DeltaVariation, 0.25, 300);
            AnimationLib.MoveToTargetY(RBase, -DeltaVariation, 0.25, 325);
            AnimationLib.MoveToTargetY(UC_Commands, -DeltaVariation, 0.25, 350);
            AnimationLib.MoveToTargetY(R1, -DeltaVariation, 0.25, 375);
            AnimationLib.MoveToTargetY(UC_Utility, -DeltaVariation, 0.25, 400);
            AnimationLib.MoveToTargetY(R2, -DeltaVariation, 0.25, 425);
            AnimationLib.MoveToTargetY(UC_Plugins, -DeltaVariation, 0.25, 450);
            AnimationLib.MoveToTargetY(R3, -DeltaVariation, 0.25, 475);
            AnimationLib.MoveToTargetY(UC_Restart, -DeltaVariation, 0.25, 500);


            AnimationLib.MoveToTargetX(BTN_LaunchServer, DeltaVariation - 100, 0.25, 100);
            AnimationLib.MoveToTargetX(listBox, DeltaVariation - 100, 0.3, 200);
            AnimationLib.MoveToTargetX(label_player, DeltaVariation - 100, 0.35, 200);
            AnimationLib.MoveToTargetX(img_Players, DeltaVariation - 100, 0.35, 230);
            AnimationLib.MoveToTargetX(BTN_Enter, -DeltaVariation * 7, 0.4, 150);
            AnimationLib.MoveToTargetX(CommandLine, -DeltaVariation * 7, 0.4, 250);
            AnimationLib.MoveToTargetX(img_Text, -DeltaVariation * 7, 0.4, 280);
            AnimationLib.MoveToTargetX(RTB_Console, -DeltaVariation * 7, 0.3, 300);
            AnimationLib.MoveToTargetX(label_console, -DeltaVariation * 7, 0.35, 300);
            AnimationLib.MoveToTargetX(img_Console, -DeltaVariation * 7, 0.35, 330);

        }
        #endregion

        #region Blur Settings

        Storyboard myStoryboard = new Storyboard();
        DoubleAnimation myDoubleAnimation = new DoubleAnimation();
        BlurEffect blurEffect = new BlurEffect();
        private void DoBlur()
        {
            if (!isBlurEnabled)
            {
                myDoubleAnimation.From = 1;
                myDoubleAnimation.To = 0.2;
                myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.25));
                BeginAnimation(OpacityProperty, myDoubleAnimation);
                return;
            }
            RegisterName("blurEffect", blurEffect);
            blurEffect.Radius = 0;
            blurEffect.RenderingBias = RenderQuality;
            Effect = blurEffect;

            myDoubleAnimation.From = 0;
            myDoubleAnimation.To = blurValue;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.125));
            myDoubleAnimation.AutoReverse = false;

            Storyboard.SetTargetName(myDoubleAnimation, "blurEffect");
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(BlurEffect.RadiusProperty));
            myStoryboard.Children.Add(myDoubleAnimation);
            myStoryboard.Begin(this);
        }

        private void DeBlur()
        {
            if (!isBlurEnabled)
            {
                myDoubleAnimation.From = 0.2;
                myDoubleAnimation.To = 1;
                myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.25));
                BeginAnimation(OpacityProperty, myDoubleAnimation);
                return;
            }
            RegisterName("blurEffect", blurEffect);
            blurEffect.Radius = 0;
            blurEffect.RenderingBias = RenderQuality;
            Effect = blurEffect;

            myDoubleAnimation.From = blurValue;
            myDoubleAnimation.To = 0;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.125));
            myDoubleAnimation.AutoReverse = false;

            Storyboard.SetTargetName(myDoubleAnimation, "blurEffect");
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(BlurEffect.RadiusProperty));
            myStoryboard.Children.Add(myDoubleAnimation);
            myStoryboard.Begin(this);
        }

        #endregion

        #endregion

        private void UC_Commands_GotFocus(object sender, RoutedEventArgs e) => MessageBox.Show("Ricevuto!");

        private void UC_Commands_LostFocus(object sender, RoutedEventArgs e) => MessageBox.Show("Sganciato!");
    }

    public class ConCatPlayers
    {
        public string PlayerIDs { get; set; }
        public string PlayerNames { get; set; }

        public override string ToString()
        {
            return string.Format("{0} : {1}", PlayerNames, PlayerIDs);
        }
    }

    static class AsyncUtils
    {
        static public void DelayCall(int msec, Action fn)
        {
            Dispatcher d = Dispatcher.CurrentDispatcher;
            new Task(() => {
                Thread.Sleep(msec);
                d.BeginInvoke(fn);
            }).Start();
        }
    }
}
