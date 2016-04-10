using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace UCS.UI
{
    /// <summary>
    /// Logica di interazione per PopupUpdater.xaml
    /// </summary>
    public partial class PopupUpdater : Window
    {
        private bool IsGoingPage = false;

        public PopupUpdater()
        {
            Opacity = 0;
            InitializeComponent();
            RTB_Console.Document.Blocks.Clear();
            RTB_Console.AppendText(Sys.ConfUCS.Changelog);
            Version thisAppVer = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            lbl_CurVer.Content = "Current UCS version: " + thisAppVer.Major + "." + thisAppVer.Minor + "." + thisAppVer.Build + "." + thisAppVer.MinorRevision;
            lbl_NewVer.Content = "New UCS version: " + Sys.ConfUCS.NewVer.Major + "." + Sys.ConfUCS.NewVer.Minor + "." + Sys.ConfUCS.NewVer.Build + "." + Sys.ConfUCS.NewVer.MinorRevision;

        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btn_GoPage_Click(object sender, RoutedEventArgs e)
        {
            IsGoingPage = true;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpInW();
           
            int DeltaVariation = 100;
            AnimationLib.MoveToTargetY(btn_Cancel, DeltaVariation, 0.25);
            AnimationLib.MoveToTargetY(btn_GoPage, DeltaVariation, 0.25, 50);
            AnimationLib.MoveToTargetY(RTB_Console, DeltaVariation, 0.25, 100);
            AnimationLib.MoveToTargetY(lbl_Changelog, DeltaVariation, 0.25, 150);
            AnimationLib.MoveToTargetY(lbl_CurVer, DeltaVariation, 0.25, 200);
            AnimationLib.MoveToTargetY(lbl_NewVer, DeltaVariation, 0.25, 250);
            AnimationLib.MoveToTargetY(lbl_Title, DeltaVariation, 0.25, 300);

            AnimationLib.MoveWindowToTargetY(this, DeltaVariation, Top, 0.25);

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
            OpOut.Completed += (s, _) => { this.Close(); MainWindow.IsFocusOk = true; if (IsGoingPage) System.Diagnostics.Process.Start(Sys.ConfUCS.UrlPage);  IsGoingPage = false; };
            BeginAnimation(OpacityProperty, OpOut);
        }


        

    }
}
