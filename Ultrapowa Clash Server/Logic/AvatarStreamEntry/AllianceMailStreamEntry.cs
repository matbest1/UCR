using System;
using System.Collections.Generic;
using System.Text;
using UCS.Helpers;

namespace UCS.Logic
{
    internal class AllianceMailStreamEntry : AvatarStreamEntry
    {
        private int m_vAllianceBadgeData;
        private long m_vAllianceId;
        private string m_vAllianceName;
        private string m_vMessage;
        private long m_vSenderId;
        
        public override byte[] Encode()
        {
            var data = new List<byte>();

            data.AddRange(base.Encode());
            data.AddInt32(2);
            data.AddString(m_vMessage);
            data.Add(1);
            data.AddInt64(m_vSenderId);
            data.AddInt64(m_vAllianceId);
            data.AddString(m_vAllianceName);
            data.AddInt32(m_vAllianceBadgeData);
            data.AddInt32(-1);

            return data.ToArray();
        }

        public string GetMessage()
        {
            return m_vMessage;
        }

        public override int GetStreamEntryType()
        {
            return 6;
        }

        public void SetAllianceBadgeData(int data)
        {
            m_vAllianceBadgeData = data;
        }

        public void SetAllianceId(long id)
        {
            m_vAllianceId = id;
        }

        public void SetAllianceName(string name)
        {
            m_vAllianceName = name;
        }

        public void SetMessage(string message)
        {
            m_vMessage = message;
        }

        public void SetSenderId(long id)
        {
            m_vSenderId = id;
        }
    }
}