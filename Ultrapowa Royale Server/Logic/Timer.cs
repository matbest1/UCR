using System;

namespace UCS.Logic
{
    internal class Timer
    {
        private int m_vSeconds;
        private DateTime m_vStartTime;

        public Timer()
        {
            m_vStartTime = new DateTime(1970, 1, 1);
            m_vSeconds = 0;
        }

        public void FastForward(int seconds)
        {
            m_vSeconds -= seconds;
        }

        public int GetRemainingSeconds(DateTime time)
        {
            var result = m_vSeconds - (int)time.Subtract(m_vStartTime).TotalSeconds;
            if (result <= 0)
                result = 0;
            return result;
        }

        public void StartTimer(int seconds, DateTime time)
        {
            m_vStartTime = time;
            m_vSeconds = seconds;
        }
    }
}