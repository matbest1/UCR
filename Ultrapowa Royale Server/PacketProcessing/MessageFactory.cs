using System;
using System.Collections.Generic;
using System.IO;

namespace UCS.PacketProcessing
{
    //Command list: LogicCommand::createCommand
    internal static class MessageFactory
    {
        private static readonly Dictionary<int, Type> m_vMessages;

        static MessageFactory()
        {
            m_vMessages = new Dictionary<int, Type>();
            m_vMessages.Add(10100, typeof (SessionRequest));
            m_vMessages.Add(10101, typeof (LoginMessage));
            m_vMessages.Add(10108, typeof (KeepAliveMessage));
            m_vMessages.Add(14102, typeof (ExecuteCommandsMessage));
        }

        public static object Read(Client c, BinaryReader br, int packetType)
        {
            if (m_vMessages.ContainsKey(packetType))
                return Activator.CreateInstance(m_vMessages[packetType], c, br);

            Console.WriteLine("[UCR]    The message '" + packetType + "' is unhandled");
            return null;
        }
    }
}