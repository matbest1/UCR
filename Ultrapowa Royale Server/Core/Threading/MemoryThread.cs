using System;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using ThreadState = System.Threading.ThreadState;
using Timer = System.Timers.Timer;

namespace UCS.Core.Threading
{
    internal class MemoryThread
    {
        private static Thread T { get; set; }

        public static void Start()
        {
            T = new Thread(() =>
            {
                var t = new Timer();
                t.Interval = 15000;
                t.Elapsed += (s, a) =>
                {
                    GC.Collect(GC.MaxGeneration);
                    GC.WaitForPendingFinalizers();
                    SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, (UIntPtr)0xFFFFFFFF,
                        (UIntPtr)0xFFFFFFFF);
                };
                t.Enabled = true;
            });
            T.Start();
        }

        public static void Stop()
        {
            if (T.ThreadState == ThreadState.Running)
                T.Abort();
        }

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetProcessWorkingSetSize(IntPtr process, UIntPtr minimumWorkingSetSize,
            UIntPtr maximumWorkingSetSize);
    }

    internal class PerformanceInfo
    {
        [DllImport("psapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetPerformanceInfo();

        public static long GetPhysicalAvailableMemoryInMiB()
        {
            var pi = new PerformanceInformation();
            if (GetPerformanceInfo())
                return Convert.ToInt64(pi.PhysicalAvailable.ToInt64() * pi.PageSize.ToInt64() / 1048576);
            return -1;
        }

        public static long GetTotalMemoryInMiB()
        {
            var pi = new PerformanceInformation();
            if (GetPerformanceInfo())
                return Convert.ToInt64(pi.PhysicalTotal.ToInt64() * pi.PageSize.ToInt64() / 1048576);
            return -1;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PerformanceInformation
        {
            public int Size;
            public IntPtr CommitTotal;
            public IntPtr CommitLimit;
            public IntPtr CommitPeak;
            public IntPtr PhysicalTotal;
            public IntPtr PhysicalAvailable;
            public IntPtr SystemCache;
            public IntPtr KernelTotal;
            public IntPtr KernelPaged;
            public IntPtr KernelNonPaged;
            public IntPtr PageSize;
            public int HandlesCount;
            public int ProcessCount;
            public int ThreadCount;
        }
    }
}