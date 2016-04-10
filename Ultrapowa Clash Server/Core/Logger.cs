using System;
using System.IO;
using System.Text.RegularExpressions;
using UCS.PacketProcessing;

namespace UCS.Core
{
    internal static class Logger
    {
        private static readonly object m_vSyncObject = new object();
        private static readonly TextWriter m_vTextWriter;
        private static int m_vLogLevel;

        /// <summary>
        /// The launcher of the Logger class.
        /// </summary>
        static Logger()
        {
            m_vTextWriter = TextWriter.Synchronized(File.AppendText("logs/data_" + DateTime.Now.ToString("yyyyMMdd") + ".log"));
            m_vLogLevel = 1;
        }

        /// <summary>
        /// This function set the logging level of the logger.
        /// </summary>
        /// <param name="level">Enum : 1, 4, 5.</param>
        public static void SetLogLevel(int level)
        {
            m_vLogLevel = level;
        }

        public static int GetLogLevel()
        {
            return m_vLogLevel;
        }

        /// <summary>
        /// This function write the specific text to the actually logging file.
        /// </summary>
        /// <param name="p">The message/packet.</param>
        /// <param name="prefix">The prefix of the log.</param>
        /// <param name="logLevel">The log level.</param>
        public static void WriteLine(Message p, string prefix = null, int logLevel = 4)
        {
            if (logLevel <= m_vLogLevel)
                lock (m_vSyncObject)
                {
                    m_vTextWriter.Write(DateTime.Now.ToString("yyyyMMddHHmmss"));
                    m_vTextWriter.Write("; ");
                    if (prefix != null)
                    {
                        m_vTextWriter.Write(prefix);
                    }
                    m_vTextWriter.Write(p.GetMessageType().ToString());
                    m_vTextWriter.Write("(");
                    m_vTextWriter.Write(p.GetMessageVersion().ToString());
                    m_vTextWriter.Write(")");
                    m_vTextWriter.Write("; ");
                    m_vTextWriter.Write(p.GetLength().ToString());
                    m_vTextWriter.Write("; ");
                    m_vTextWriter.WriteLine(p.ToHexString());
                    m_vTextWriter.WriteLine(Regex.Replace(p.ToString(), @"[^\u0020-\u007F]", "."));
                    m_vTextWriter.Flush();
                }
        }
        /// <summary>
        /// This function write the specific string in the logging file.
        /// </summary>
        /// <param name="s">The string to add.</param>
        /// <param name="prefix">The prefix of the log.</param>
        /// <param name="logLevel">The log level.</param>
        public static void WriteLine(string s, string prefix = null, int logLevel = 4)
        {
            if (logLevel <= m_vLogLevel)
            {
                lock (m_vSyncObject)
                {
                    m_vTextWriter.Write("{0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
                    m_vTextWriter.Write("; ");
                    if (prefix != null)
                    {
                        m_vTextWriter.Write(prefix);
                    }
                    m_vTextWriter.WriteLine(s);
                    m_vTextWriter.Flush();
                }
            }
        }
    }
}