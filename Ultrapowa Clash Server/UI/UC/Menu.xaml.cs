using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace UCS.UI.UC
{
    /// <summary>
    /// Logica di interazione per Menu.xaml
    /// </summary>
    public partial class Menu : UserControl
    {
        public RoutedEvent ClickEvent;
        public RoutedEvent OverEvent;
        public RoutedEvent RetireEvent;

        public Menu()
        {
            InitializeComponent();
            ClickEvent = ButtonBase.ClickEvent.AddOwner(typeof(Menu));
            OverEvent = MouseEnterEvent.AddOwner(typeof(Menu));
            RetireEvent = MouseLeaveEvent.AddOwner(typeof(Menu));

            Background = new SolidColorBrush(Color.FromRgb(0x00, 0x77, 0x9F));
            var RT = new RotateTransform(90);
            Arrow.RenderTransformOrigin = new Point(0.5, 0.5);
            Arrow.RenderTransform = RT;
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

        #endregion

        #region Properties
        public string NameLabel
        {
            get
            {
                return l_Name.Content.ToString();
            }
            set
            {
                l_Name.Content = value;
                if (IsArrowEnabled)
                {
                    l_Name.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    Arrow.Margin = new Thickness(0, 0, -l_Name.DesiredSize.Width - 16, 0);
                    ACAB.Margin = new Thickness(-24, 0, 0, 0);
                }  
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

        public ImageSource ImageArrow
        {
            get
            {
                return Arrow.Source;
            }
            set
            {
                Arrow.Source = value;
                IsArrowEnabled = true;
            }
        }

        private bool _IsArrowEnabled = false;
        public bool IsArrowEnabled
        {
            get
            {
                return _IsArrowEnabled;
            }
            set
            {
                _IsArrowEnabled = value;
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
                {
                    AnimationLib.RotateImage(Arrow, 180, 0.25);
                    AnimationLib.ChangeBackgroundColor(this, Color.FromRgb(0x00, 0x4c, 0x65), 0.2);
                }
                else
                {
                    AnimationLib.RotateImage(Arrow, 90, 0.25);
                    AnimationLib.ChangeBackgroundColor(this, Color.FromRgb(0x00, 0x77, 0x9F), 0.2);
                }
            }
        }
        #endregion
    }
}
