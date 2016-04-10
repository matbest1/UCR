using Sodium;
using System;
using System.Reflection;
using System.Threading;

namespace UCKG
{
    internal class Program : IDisposable
    {
        public static string Author = "Aidid";
        public static string Description = "Generate Public And Private Key For UCS";
        public static string Name = "Ultrapowa";
        public static string Version = "1.0.0";
        private static string _title, _tmp;
        private static bool kng;
        private static bool _disposed;
        private static Thread T { get; set; }

        internal static void Main()
        {
            T = new Thread(() =>
            {
                /* Animated Console Title */
                _title = "Ultrapowa Key Generator " + " v" + Assembly.GetExecutingAssembly().GetName().Version;
                _tmp = string.Empty;
                for (var i = 0; i < _title.Length; i++)
                {
                    _tmp += _title[i];
                    Console.Title = _tmp;
                    Thread.Sleep(40);
                }
                /* ASCII Art centered */
                Console.WriteLine(
                    @"
    888     888 888    88888888888 8888888b.         d8888 8888888b.   .d88888b.  888       888        d8888
    888     888 888        888     888   Y88b       d88888 888   Y88b d88P' 'Y88b 888   o   888       d88888
    888     888 888        888     888    888      d88P888 888    888 888     888 888  d8b  888      d88P888
    888     888 888        888     888   d88P     d88P 888 888   d88P 888     888 888 d888b 888     d88P 888
    888     888 888        888     8888888P'     d88P  888 8888888P'  888     888 888d88888b888    d88P  888
    888     888 888        888     888 T88b     d88P   888 888        888     888 88888P Y88888   d88P   888
    Y88b. .d88P 888        888     888  T88b   d8888888888 888        Y88b. .d88P 8888P   Y8888  d8888888888
     'Y88888P'  88888888   888     888   T88b d88P     888 888         'Y88888P'  888P     Y888 d88P     888
                  ");
                Console.WriteLine("[UCKG]    -> This Program is made by the {0} Developer Team!", Name);
                Console.WriteLine("[UCKG]    -> {0} Key Generator is now generating key..", Name);
                kng = true;
                while (kng)
                {
                    var key = PublicKeyBox.GenerateKeyPair();
                    Console.WriteLine("[UCKG]    -> Public Key  = 0x" +
                                      BitConverter.ToString(key.PublicKey).Replace("-", ", 0x"));
                    Console.WriteLine("[UCKG]    -> Private Key = 0x" +
                                      BitConverter.ToString(key.PrivateKey).Replace("-", ", 0x"));
                    kng = false;
                }
                Console.WriteLine("[UCKG]    -> Need other key? Press y or if you want to exit press N");
                var result = Console.ReadKey();
                switch (Char.ToUpper(result.KeyChar))
                {
                    case 'Y':
                        Console.Clear();
                        Main();
                        break;

                    case 'N':
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("[UCKG]    Unknown Key Pressed. Please Choose Between Y or N.");
                        break;
                }
            });
            T.Start();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    try
                    {
                        throw new ObjectDisposedException(null, "Cannot generate key because generator have been disposed.");
                    }
                    catch { }
                }
                _disposed = true;
            }
        }
    }
}
