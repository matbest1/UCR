using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Reflection;
using System.Configuration;

namespace UCS.Core
{
    class HTTP
    {
        private Thread _serverThread;
        private HttpListener _listener;
        private int _port;
        private string jsonapp;
        private string mime = "text/plain";

        public int Port
        {
            get { return _port; }
            private set { }
        }

        /// <summary>
        /// Construct server with given port.
        /// </summary>
        /// <param name="path">Directory path to serve.</param>
        /// <param name="port">Port of the server.</param>
        public HTTP(int port)
        {
            this.Initialize(port);
        }

        /// <summary>
        /// Construct server with suitable port.
        /// </summary>
        /// <param name="path">Directory path to serve.</param>
        public HTTP()
        {
            //get an empty port
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            this.Initialize(port);
            
        }

        /// <summary>
        /// Stop server and dispose all functions.
        /// </summary>
        public void Stop()
        {
            _serverThread.Abort();
            _listener.Stop();
        }

        private void Listen()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://+:" + _port.ToString() + "/" + ConfigurationManager.AppSettings["ApiKey"] + "/");
            _listener.Start();
            while (true)
            {
                try
                {
                    HttpListenerContext context = _listener.GetContext();
                    Process(context);
                }
                catch (Exception ex)
                {
                    // We should do nothing
                }
            }
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private void Handler(string type)
        {
            try
            {
                if (type == "inmemclans")
                    jsonapp = Convert.ToString(ObjectManager.GetInMemoryAlliances().Count);
                else if (type == "inmemplayers")
                    jsonapp = Convert.ToString(ResourcesManager.GetInMemoryLevels().Count);
                else if (type == "onlineplayers")
                    jsonapp = Convert.ToString(ResourcesManager.GetOnlinePlayers().Count);
                else if (type == "totalclients")
                    jsonapp = Convert.ToString(ResourcesManager.GetConnectedClients().Count);
                else if (type == "all")
                {
                    var json = new JsonApi
                    {
                        UCS = new Dictionary<string, string>
                            {
                                {"PatchingServer", ConfigurationManager.AppSettings["patchingServer"]},
                                {"Maintenance", ConfigurationManager.AppSettings["maintenanceMode"]},
                                {"MaintenanceTimeLeft", ConfigurationManager.AppSettings["maintenanceTimeLeft"]},
                                {"ClientVersion", ConfigurationManager.AppSettings["clientVersion"]},
                                {"ServerVersion", Assembly.GetExecutingAssembly().GetName().Version.ToString()},
                                {"OnlinePlayers", Convert.ToString(ResourcesManager.GetOnlinePlayers().Count)},
                                {"InMemoryPlayers", Convert.ToString(ResourcesManager.GetInMemoryLevels().Count)},
                                {"InMemoryClans", Convert.ToString(ObjectManager.GetInMemoryAlliances().Count)},
                                {"TotalConnectedClients", Convert.ToString(ResourcesManager.GetConnectedClients().Count)}
                            }
                    };
                    jsonapp = JsonConvert.SerializeObject(json);
                    mime = "application/json";
                }
                else if (type == "ram")
                {
                    jsonapp = Performances.GetUsedMemory();
                }
                else
                    jsonapp = "OK";
            }
            catch (Exception ex)
            {
                jsonapp = "An exception occured in UCS : \n" + ex;
            }
        }

        private void Process(HttpListenerContext context)
        {
            string[] Apis = new string[] { "inmemclans", "inmemplayers", "onlineplayers", "totalclients", "ram", ""};
            string type = context.Request.Url.AbsolutePath.Substring(7).ToLower();

            if (Apis.Contains(type))
            {
                Handler(type);
                try
                {
                    context.Response.ContentType = mime;
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    context.Response.AddHeader("Date", DateTime.Now.ToString("r"));
                    context.Response.AddHeader("Last-Modified", DateTime.UtcNow.ToString("r"));
                    context.Response.AddHeader("APIVersion", "1.0a");

                    byte[] buffer = new byte[1024 * 16];
                    int nbytes;

                    using (Stream fstream = GenerateStreamFromString(jsonapp))
                    {
                        while ((nbytes = fstream.Read(buffer, 0, buffer.Length)) > 0)
                            context.Response.OutputStream.Write(buffer, 0, nbytes);
                        fstream.Close();
                    }
                    
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.OutputStream.Flush();
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            context.Response.OutputStream.Close();
        }
        

        private void Initialize(int port)
        {
            this._port = port;
            _serverThread = new Thread(this.Listen);
            _serverThread.Start();
            Console.WriteLine("[UCS]    API has been successfully started");
        }
        public Dictionary<string, string> UCS { get; internal set; }
    }

    public struct JsonApi
    {
        public Dictionary<string, string> UCS { get; set; }
    }
}