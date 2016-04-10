using System;
using System.Xaml;
using System.Windows;

namespace UCS_Editor
{
    /// <summary>
    /// Logica di interazione per UserControl1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("This plugin works!");
        }
    }
}
