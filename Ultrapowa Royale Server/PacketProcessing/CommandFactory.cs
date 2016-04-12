using System;
using System.Collections.Generic;
using System.IO;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Command list: LogicCommand::createCommand
    internal static class CommandFactory
    {
        private static readonly Dictionary<uint, Type> m_vCommands;

        static CommandFactory()
        {
            m_vCommands = new Dictionary<uint, Type>();
        }

        public static object Read(BinaryReader br)
        {
            var cm = br.ReadUInt32WithEndian();
            if (m_vCommands.ContainsKey(cm))
                return Activator.CreateInstance(m_vCommands[cm], br);
            Console.WriteLine("\t The command '" + cm + "' has been ignored");
            return null;
        }
    }
}