using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using UCS.Sys;
using UCS.UI.UC;

namespace UCS.UI
{
    /// <summary>
    /// Logica di interazione per PopupPluginManager.xaml
    /// </summary>
    public partial class PopupPluginManager : Window
    {

        public Item CurrentSelectedElement;

        public PopupPluginManager()
        {
            InitializeComponent();
        }

        int DeltaVariation = 300;

        private void RefreshList()
        {

            double count = 0;
            double maxCount = ConfUCS.PM.LoadedPluginsICP.Count + ConfUCS.PM.LoadedPluginsIGP.Count;
            for (int i = 0; i < ConfUCS.PM.LoadedPluginsICP.Count; i++)
            {
                var itm = new Item();
                itm.ICP = ConfUCS.PM.LoadedPluginsICP[i];
                itm.NameLabel = ConfUCS.PM.LoadedPluginsICP[i].plugin.Title;
                itm.Margin = new Thickness(3, 3, 3, 3);
                count++;
                SP.Children.Add(itm);
                PB_Loader.Value = (count / maxCount) * 100D;
                itm.MouseEnter += Item_MouseEnter;
                itm.MouseLeave += Item_MouseLeave;
                itm.Click += Item_ICP_Click;
                AnimationLib.MoveToTargetX(itm, SP.RenderSize.Width, 0.25);
            }
            for (int i = 0; i < ConfUCS.PM.LoadedPluginsIGP.Count; i++)
            {
                var itm = new Item();
                itm.IGP = ConfUCS.PM.LoadedPluginsIGP[i];
                itm.NameLabel = ConfUCS.PM.LoadedPluginsIGP[i].plugin.Title;
                itm.Margin = new Thickness(3, 3, 3, 3);
                count++;
                SP.Children.Add(itm);
                PB_Loader.Value = (count / maxCount) * 100D; 
                itm.MouseEnter += Item_MouseEnter;
                itm.MouseLeave += Item_MouseLeave;
                itm.Click += Item_IGP_Click;
                AnimationLib.MoveToTargetX(itm, SP.RenderSize.Width, 0.25);
            }
            PB_Loader.Visibility = Visibility.Hidden;
            lbl_Loading.Visibility = Visibility.Hidden;
        }

        private void Item_MouseEnter(object sender, MouseEventArgs e) => AnimationLib.ChangeBackgroundColor((sender as Item), Color.FromRgb(0x00, 0x4c, 0x65), 0.2);

        private void Item_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!(sender as Item).IsPressed)
                AnimationLib.ChangeBackgroundColor((sender as Item), Color.FromRgb(0x00, 0x77, 0x9F), 0.2);
        }

        private void Item_ICP_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSelectedElement != null) CurrentSelectedElement.IsPressed = false;
            CurrentSelectedElement = sender as Item;
            CurrentSelectedElement.IsPressed = true;
            lbl_Title.Content = "Title: " + CurrentSelectedElement.ICP.plugin.Title;
            lbl_AuthorName.Content = "Author name: " + CurrentSelectedElement.ICP.plugin.AuthorName;
            lbl_Version.Content = "Version: " + CurrentSelectedElement.ICP.plugin.Version;
            Uri uri;
            HT.NavigateUri = (Uri.TryCreate(CurrentSelectedElement.ICP.plugin.URL,UriKind.Absolute, out uri)) ? uri : null;

            txt_Description.Text = CurrentSelectedElement.ICP.plugin.Information;
            btn_Launch.IsEnabled = false;
        }

        private void Item_IGP_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSelectedElement != null) CurrentSelectedElement.IsPressed = false;
            CurrentSelectedElement = sender as Item;
            CurrentSelectedElement.IsPressed = true;
            lbl_Title.Content = "Title: " + CurrentSelectedElement.IGP.plugin.Title;
            lbl_AuthorName.Content = "Author name: " + CurrentSelectedElement.IGP.plugin.AuthorName;
            lbl_Version.Content = "Version: " + CurrentSelectedElement.IGP.plugin.Version;
            Uri uri;
            HT.NavigateUri = (Uri.TryCreate(CurrentSelectedElement.IGP.plugin.URL, UriKind.Absolute, out uri)) ? uri : null;
            txt_Description.Text = CurrentSelectedElement.IGP.plugin.Information;
            btn_Launch.IsEnabled = true;
        }

        private void btn_Launch_Click(object sender, RoutedEventArgs e) => CurrentSelectedElement.IGP.LaunchUI();

        #region Events
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpInW();
            AnimationLib.MoveToTargetY(btn_Launch, DeltaVariation, 0.25);
            AnimationLib.MoveToTargetY(btn_ok, DeltaVariation, 0.25, 50);
            AnimationLib.MoveToTargetY(LB_Main, DeltaVariation/2, 0.25, 100);
            AnimationLib.MoveToTargetY(img_Plugins, DeltaVariation/2, 0.25, 150);

            AnimationLib.MoveToTargetX(lbl_Plugins, DeltaVariation, 0.25);
            AnimationLib.MoveToTargetX(lbl_Info, -DeltaVariation, 0.25,25);
            AnimationLib.MoveToTargetX(SP, DeltaVariation, 0.25,50);
            AnimationLib.MoveToTargetX(RT, DeltaVariation, 0.25, 50);
            AnimationLib.MoveToTargetX(RT2, -DeltaVariation, 0.25, 75);
            AnimationLib.MoveToTargetX(txt_Description, -DeltaVariation, 0.25, 75);
            AnimationLib.MoveToTargetX(lbl_Title, -DeltaVariation, 0.25, 100);
            AnimationLib.MoveToTargetX(lbl_AuthorName, -DeltaVariation, 0.25, 120);
            AnimationLib.MoveToTargetX(lbl_Version, -DeltaVariation, 0.25, 140);
            AnimationLib.MoveToTargetX(lbl_URL, -DeltaVariation, 0.25, 160);
            AnimationLib.MoveToTargetX(HyperLink, -DeltaVariation, 0.25, 160);
            AnimationLib.MoveToTargetX(lbl_Description, -DeltaVariation, 0.25, 180);

            AnimationLib.MoveWindowToTargetY(this, 100, Top, 0.25);
            RefreshList();
        }

        private void OpInW()
        {
            var OpIn = new DoubleAnimation(0,1, TimeSpan.FromSeconds(0.25));
            BeginAnimation(OpacityProperty, OpIn);
        }

        private void OpOutW(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Closing -= Window_Closing;
            e.Cancel = true;
            var OpOut = new DoubleAnimation(0, TimeSpan.FromSeconds(0.25));
            OpOut.Completed += (s, _) => { this.Close(); MainWindow.IsFocusOk = true; };
            BeginAnimation(OpacityProperty, OpOut);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => OpOutW(sender, e);

        private void btn_ok_Click(object sender, RoutedEventArgs e) => Close();
        #endregion

        private void HL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start((sender as Hyperlink).NavigateUri.AbsoluteUri);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something goes wrong: {0}", ex.Message);
            }
            
        }
    }
}
