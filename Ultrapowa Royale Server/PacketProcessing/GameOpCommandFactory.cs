using System;
using System.Collections.Generic;

namespace UCS.PacketProcessing
{
    internal static class GameOpCommandFactory
    {
        private static readonly Dictionary<string, Type> m_vCommands;

        static GameOpCommandFactory()
        {
            m_vCommands = new Dictionary<string, Type>();
        }

        public static object Parse(string command)
        {
            var commandArgs = command.Split(' ');
            object result = null;
            if (commandArgs.Length > 0)
            {
                if (m_vCommands.ContainsKey(commandArgs[0]))
                {
                    var type = m_vCommands[commandArgs[0]];
                    var ctor = type.GetConstructor(new[] { typeof(string[]) });
                    result = ctor.Invoke(new object[] { commandArgs });
                }
            }
            return result;
        }
    }
}