using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;

namespace UCS.Core.Threading
{
    class InterfaceThread
    {
        /// <summary>
        /// Variable holding the thread itself
        /// </summary>
        private static Thread T { get; set; }

        /// <summary>
        /// Starts the Thread
        /// </summary>
        [STAThread]
        public static void Start()
        {
            T = new Thread(() =>
            {
                MainWindow.RemoteWindow.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    MainWindow.RemoteWindow.Show();
                });      
            });
            T.SetApartmentState(ApartmentState.STA); //Required running in single thread mode
            T.Start();
        }

        /// <summary>
        /// Stops the Thread
        /// </summary>
        public static void Stop()
        {
            if (T.ThreadState == ThreadState.Running)
                T.Abort();
        }
    }
}
