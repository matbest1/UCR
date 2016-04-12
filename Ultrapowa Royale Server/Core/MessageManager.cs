using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UCS.Logic;
using UCS.PacketProcessing;

namespace UCS.Core
{
    internal class MessageManager
    {
        private static ConcurrentQueue<Message> m_vPackets;
        private static EventWaitHandle m_vWaitHandle = new AutoResetEvent(false);
        private bool m_vIsRunning;

        private delegate void PacketProcessingDelegate();

        /// <summary>
        /// The loader of the MessageManager class.
        /// </summary>
        public MessageManager()
        {
            m_vPackets = new ConcurrentQueue<Message>();
            m_vIsRunning = false;
        }

        /// <summary>
        /// This function start the MessageManager.
        /// </summary>
        public void Start()
        {
            PacketProcessingDelegate packetProcessing = new PacketProcessingDelegate(PacketProcessing);
            packetProcessing.BeginInvoke(null, null);
            m_vIsRunning = true;
            Console.WriteLine("[UCR]    Message manager has been successfully started !");
        }

        /// <summary>
        /// This function process packets.
        /// </summary>
        private void PacketProcessing()
        {
            while (m_vIsRunning)
            {
                m_vWaitHandle.WaitOne();

                Message p;
                while (m_vPackets.TryDequeue(out p))
                {
                    Level pl = p.Client.GetLevel();
                    string player = "";
                    if (pl != null)
                        player += " (" + pl.GetPlayerAvatar().GetId() + ", " + pl.GetPlayerAvatar().GetAvatarName() + ")";
                    try
                    {
                        Debugger.WriteLine("[UCR][" + p.GetMessageType() + "] Processing " + p.GetType().Name + player);
                        p.Decode();
                        p.Process(pl);
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Debugger.WriteLine("[UCR][" + p.GetMessageType() + "] An exception occured during processing of message " + p.GetType().Name + player, ex);
                        Console.ResetColor();
                    }
                }
            }
        }

        /// <summary>
        /// This function handle the packet by enqueue him.
        /// </summary>
        /// <param name="p">The message/packet.</param>
        public static void ProcessPacket(Message p)
        {
            m_vPackets.Enqueue(p);
            m_vWaitHandle.Set();
        }
    }
}