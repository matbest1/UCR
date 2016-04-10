using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using UCS.Core.Interfaces;
using UCS.Sys;

namespace UCS.UI.UC
{
    /// <summary>
    /// Logica di interazione per Item.xaml
    /// </summary>
    public partial class Item : UserControl
    {
        public RoutedEvent ClickEvent;
        public RoutedEvent OverEvent;
        public RoutedEvent RetireEvent;

        public Item()
        {
            InitializeComponent();
            ClickEvent = ButtonBase.ClickEvent.AddOwner(typeof(Menu));
            OverEvent = MouseEnterEvent.AddOwner(typeof(Menu));
            RetireEvent = MouseLeaveEvent.AddOwner(typeof(Menu));
            Background = new SolidColorBrush(Color.FromRgb(0x00, 0x77, 0x9F));
        }

        #region Events

        public event RoutedEventHandler Retire
        {
            add { AddHandler(OverEvent, value); }
            remove { RemoveHandler(OverEvent, value); }
        }

        public event RoutedEventHandler Over
        {
            add { AddHandler(OverEvent, value); }
            remove { RemoveHandler(OverEvent, value); }
        }

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            CaptureMouse();
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (IsMouseCaptured)
            {
                ReleaseMouseCapture();
                if (IsMouseOver)
                    RaiseEvent(new RoutedEventArgs(ClickEvent, this));
            }
        }

        public void RunUI()
        {
            throw new NotImplementedException();
        }

        #endregion

        public string NameLabel
        {
            get
            {
                return l_Name.Content.ToString();
            }
            set
            {
                l_Name.Content = value;
            }
        }
        public ImageSource ImageLink
        {
            get
            {
                return Icon.Source;
            }
            set
            {
                Icon.Source = value;
            }
        }
        private bool _IsPressed = false;
        public bool IsPressed
        {
            get
            {
                return _IsPressed;
            }
            set
            {
                _IsPressed = value;
                if (value)
                    AnimationLib.ChangeBackgroundColor(this, Color.FromRgb(0x00, 0x4c, 0x65), 0.2);
                else
                    AnimationLib.ChangeBackgroundColor(this, Color.FromRgb(0x00, 0x77, 0x9F), 0.2);
            }
        }

        public PluginWrapperIGP IGP;
        public PluginWrapperICP ICP;
        
    }
}
