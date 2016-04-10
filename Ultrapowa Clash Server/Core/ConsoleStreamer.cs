using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Threading;

namespace UCS.Core
{
    public class ConsoleStreamer : TextWriter
    {

        TextBox TB = null;
        public static string Value;

        public ConsoleStreamer(TextBox output)
        {
            TB = output;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Write(char value)
        {
            Value += value.ToString();
            if (value.ToString() == "\n")
            {
                TB.Dispatcher.BeginInvoke(new Action(() =>
                {
                    TB.Text = Value;
                    TB.Select(TB.Text.Length, 0);
                    TB.ScrollToEnd();
                }), DispatcherPriority.Send);
            }
        }

        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }
    }
}
