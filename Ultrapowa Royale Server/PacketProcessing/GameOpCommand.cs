using UCS.Logic;
using UCS.Network;

namespace UCS.PacketProcessing
{
    internal class GameOpCommand
    {
        private byte m_vRequiredAccountPrivileges;

        public virtual void Execute(Level level)
        {
        }

        public byte GetRequiredAccountPrivileges()
        {
            return m_vRequiredAccountPrivileges;
        }

        public static void SendCommandFailedMessage(Client c)
        {
        }

        public void SetRequiredAccountPrivileges(byte level)
        {
            m_vRequiredAccountPrivileges = level;
        }
    }
}