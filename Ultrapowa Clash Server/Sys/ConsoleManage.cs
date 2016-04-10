using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UCS.Sys
{
    public class ConsoleManage
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FreeConsole();

        [DllImport("kernel32", SetLastError = true)]
        public static extern bool AttachConsole(int dwProcessId);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        public const uint SW_HIDE = 0;
        public const uint SW_SHOWNORMAL = 1;
        public const uint SW_SHOWNOACTIVATE = 4;

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        public static extern IntPtr DeleteMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        public const uint SC_CLOSE = 0xF060;
        public const uint MF_BYCOMMAND = (uint)0x00000000L;

        public static bool ConsoleVisible { get; set; }

        public static void HideConsole()
        {
            IntPtr handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
            ConsoleVisible = false;

        }
        public static void ShowConsole(bool active = true)
        {
            IntPtr handle = GetConsoleWindow();
            if (active) { ShowWindow(handle, SW_SHOWNORMAL); }
            else { ShowWindow(handle, SW_SHOWNOACTIVATE); }
            ConsoleVisible = true;
        }

        // Disable Console Exit Button

        public static void DisableConsoleExit()
        {
            //IntPtr handle = GetConsoleWindow();
            //IntPtr exitButton = GetSystemMenu(handle, false);
            //if (exitButton != null) DeleteMenu(exitButton, SC_CLOSE, MF_BYCOMMAND);
        }

    }
}
