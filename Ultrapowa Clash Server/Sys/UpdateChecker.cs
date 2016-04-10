using System;
using System.Reflection;
using System.Threading;
using System.Xml;
using UCS.UI;

namespace UCS.Sys
{
    class UpdateChecker
    {
        public static void Check()
        {
            var NamesEL = "";
            XmlTextReader ReadTheXML = null;

            try
            {
                ReadTheXML = new XmlTextReader(ConfUCS.UrlXML);
                ReadTheXML.MoveToContent();
                if ((ReadTheXML.NodeType == XmlNodeType.Element) && (ReadTheXML.Name == "appinfo"))
                    while (ReadTheXML.Read())
                        if (ReadTheXML.NodeType == XmlNodeType.Element) NamesEL = ReadTheXML.Name;
                        else
                        {
                            if ((ReadTheXML.NodeType == XmlNodeType.Text) && (ReadTheXML.HasValue))
                            {
                                switch (NamesEL)
                                {
                                    case "version": ConfUCS.NewVer = new Version(ReadTheXML.Value); break;
                                    case "url": ConfUCS.UrlPage = ReadTheXML.Value; break;
                                    case "about": ConfUCS.Changelog = ReadTheXML.Value; break;
                                }
                            }
                        }
            }
            catch
            {
                Thread.Sleep(500);
            }
            finally
            {
                if (ReadTheXML != null) ReadTheXML.Close();
            }

            Version thisAppVer = Assembly.GetExecutingAssembly().GetName().Version;

            if (thisAppVer.CompareTo(ConfUCS.NewVer) < 0)
            {

                SplashScreen.SS.Dispatcher.BeginInvoke((Action)delegate () {
                    SplashScreen.SS.PB_Loader.Value = 90;
                    SplashScreen.SS.label_txt.Content = "New update is available.";
                });

                ConfUCS.IsUpdateAvailable = true;
            }

            else
            {

                SplashScreen.SS.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    SplashScreen.SS.label_txt.Content = "No update found";
                });

            }

        }
    }
}
