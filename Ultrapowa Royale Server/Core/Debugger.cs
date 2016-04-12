using System;
using System.IO;

namespace UCS.Core
{
    internal static class Debugger
    {
        private static readonly object m_vSyncObject = new object();
        private static readonly TextWriter m_vTextWriter;
        private static int m_vLogLevel;

        /// <summary>
        /// This is the loader of the Debugger class.
        /// </summary>
        static Debugger()
        {
            m_vTextWriter = TextWriter.Synchronized(File.AppendText("logs/debug_" + DateTime.Now.ToString("yyyyMMdd") + ".log"));
            m_vLogLevel = 1;
        }

        /// <summary>
        /// This function set the log level.
        /// </summary>
        /// <param name="level">Enum : 1, 4, 5.</param>
        public static void SetLogLevel(int level)
        {
            m_vLogLevel = level;
        }

        /// <summary>
        /// This function write the specific text and exception in the log file and on the console.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="ex">The exception.</param>
        /// <param name="logLevel">The log level.</param>
        public static void WriteLine(string text, Exception ex = null, int logLevel = 4)
        {
            var content = text;
            if (ex != null)
                content += ex.ToString();
            Console.WriteLine(content);
            if (logLevel <= m_vLogLevel)
                lock (m_vSyncObject)
                {
                    m_vTextWriter.Write(DateTime.Now.ToString("yyyyMMddHHmmss"));
                    m_vTextWriter.Write("\t");
                    m_vTextWriter.WriteLine(content);
                    if (ex != null)
                        m_vTextWriter.WriteLine(ex.ToString());
                    m_vTextWriter.Flush();
                }
        }
    }
}