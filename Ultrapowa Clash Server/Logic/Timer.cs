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

        public int GetRemainingSeconds(DateTime time, bool boost = false, DateTime boostEndTime = default(DateTime),
                                       float multiplier = 0f)
        {
            var result = int.MaxValue;
            if (!boost)
                result = m_vSeconds - (int)time.Subtract(m_vStartTime).TotalSeconds;
            else
            {
                if (boostEndTime >= time)
                    result = m_vSeconds - (int)(time.Subtract(m_vStartTime).TotalSeconds * multiplier);
                else
                {
                    var boostedTime = (float)time.Subtract(m_vStartTime).TotalSeconds -
                                      (float)(time - boostEndTime).TotalSeconds;
                    var notBoostedTime = (float)time.Subtract(m_vStartTime).TotalSeconds - boostedTime;

                    result = m_vSeconds - (int)(boostedTime * multiplier + notBoostedTime);
                }
            }
            if (result <= 0)
                result = 0;
            return result;
        }

        public int GetRemainingSeconds(DateTime time)
        {
            var result = m_vSeconds - (int)time.Subtract(m_vStartTime).TotalSeconds;
            if (result <= 0)
                result = 0;
            return result;
        }

        public DateTime GetStartTime()
        {
            return m_vStartTime;
        }
        public void StartTimer(int seconds, DateTime time)
        {
            m_vStartTime = time;
            m_vSeconds = seconds;
        }
    }
}