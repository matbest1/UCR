using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Sys;

namespace UCS.Core
{
    class AllocateConsole //For Console support for WPF
    {
        public static TextWriter StandardConsole;
        public static void Allocate(bool IsHide = false)
        {
            IntPtr ptr = ConsoleManage.GetForegroundWindow();
            int u;
            ConsoleManage.GetWindowThreadProcessId(ptr, out u);
            Process process = Process.GetProcessById(u);

            if (process.ProcessName == "cmd") ConsoleManage.AttachConsole(process.Id);
            else ConsoleManage.AllocConsole();

            ConsoleManage.DisableConsoleExit();

            if (IsHide) ConsoleManage.HideConsole();
        }

        public static void GetConsoleValue() => StandardConsole = Console.Out;
    }
}
