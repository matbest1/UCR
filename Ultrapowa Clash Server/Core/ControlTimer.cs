using System;
using System.Diagnostics;
using System.Timers;
using UCS.Sys;

namespace UCS.Core
{
    class ControlTimer
    {

        static Timer UpdateInfo = new Timer();
        static Stopwatch HighPrecisionUpdateTimer = new Stopwatch();
        static Stopwatch PerformanceCounter = new Stopwatch();
        public static string ElapsedTime = "";

        public static void Setup()
        {
            //For console
            if (ConfUCS.IsConsoleMode)
            {
                UpdateInfo.Elapsed += UpdateInfo_Tick;
                UpdateInfo.Interval = 1000;
            }
            else
            {
                //For GUI
                MainWindow.RemoteWindow.PrepareTimer();
            }
        }
        private static void UpdateInfo_Elapsed(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public static void SwitchTimer()
        {
            if (ConfUCS.IsConsoleMode)
            {
                UpdateInfo.Elapsed += UpdateInfo_Tick;
                UpdateInfo.Interval = 1000;
                UpdateInfo.Start();
            }   
            else
            {
                MainWindow.RemoteWindow.PrepareTimer();
                MainWindow.RemoteWindow.UpdateInfoGUI.Start();
            }
                
        }

        public static void StartUpTimer()
        {
            HighPrecisionUpdateTimer.Start();
            if (ConfUCS.IsConsoleMode) UpdateInfo.Start();
            else MainWindow.RemoteWindow.UpdateInfoGUI.Start();
        }

        public static void StartPerformanceCounter()
        {
            if (Stopwatch.IsHighResolution) Console.WriteLine(Properties.Resources.UsingHighTimer);
            else Console.WriteLine(Properties.Resources.UsingBasicTimer);
            Console.WriteLine(Properties.Resources.NowMeasuringTime);
            PerformanceCounter.Start();
        }

        public static void StopPerformanceCounter()
        {
            PerformanceCounter.Stop();
            TimeSpan PCTS = PerformanceCounter.Elapsed;
            string MeasuredPerformanceTime = string.Format("{0}", PCTS.TotalMilliseconds);
            Console.WriteLine(string.Format(Properties.Resources.OperationCompletedIn + " {0} ms", MeasuredPerformanceTime)); 
        }

        public static void UpdateTime()
        {
            TimeSpan ts = HighPrecisionUpdateTimer.Elapsed;
            ElapsedTime = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);

            string OutTitle = ConfUCS.UnivTitle + " | " + Properties.Resources.UpTime + " " + ElapsedTime;

            if (ConfUCS.IsConsoleMode) Console.Title = OutTitle;

            MainWindow.RemoteWindow.Title = OutTitle;
            MainWindow.RemoteWindow.LBL_UpTime.Content = Properties.Resources.UpTime + " " + ElapsedTime;
        }

        private static void UpdateInfo_Tick(object sender, EventArgs e)
        {
            UpdateTime();
        }
    }
}
