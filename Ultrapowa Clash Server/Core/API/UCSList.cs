using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Text;
using System.Threading;

namespace UCS.Core
{
    internal class UCSList
    {
        private static readonly string APIKey = ConfigurationManager.AppSettings["UCSList - APIKey"];
        private static readonly int Status = CheckStatus();
        private static readonly string UCSPanel = "https://www.ultrapowa.xyz/api/";

        public UCSList()
        {
            if (!string.IsNullOrEmpty(APIKey) && APIKey.Length == 25)
            {
                T = new Thread(() =>
                {
                    while (true)
                    {
                        SendData();
                        Thread.Sleep(60000);
                    }
                });
                T.Start();
            }
            else
                Console.WriteLine("[UCS]     UCSList API is disabled - Visit www.ultrapowa.xyz for more info.");
        }

        private static Thread T { get; set; }

        public static int CheckStatus()
        {
            var stat = Convert.ToBoolean(ConfigurationManager.AppSettings["maintenanceMode"]);
            if (stat)
                return 2;
            return 1;
        }

        public static void SendData()
        {
            var result = Http.Post(UCSPanel, new NameValueCollection
            {
                {"ApiKey", APIKey},
                {"OnlinePlayers", Convert.ToString(ResourcesManager.GetOnlinePlayers().Count)},
                {nameof(Status), Convert.ToString(Status)}
            }).Remove(0, 1);

            if (result == "OK")
                Console.WriteLine("[UCS]    UCS Sent data successfully.");
            else
                Console.WriteLine("[UCS]    UCSList Server answer uncorrectly : " + result);
        }

        public static class Http
        {
            public static string Post(string uri, NameValueCollection pairs)
            {
                byte[] response = null;
                using (var client = new WebClient())
                {
                    response = client.UploadValues(uri, pairs);
                }
                return Encoding.UTF8.GetString(response);
            }
        }
    }
}