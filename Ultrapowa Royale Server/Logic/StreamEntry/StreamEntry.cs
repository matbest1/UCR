using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UCS.Helpers;

namespace UCS.Logic
{
    internal class StreamEntry
    {
        private long m_vHomeId;
        private int m_vId;
        private DateTime m_vMessageTime;
        private long m_vSenderId;
        private int m_vSenderLeagueId;
        private int m_vSenderLevel;
        private string m_vSenderName;
        private int m_vSenderRole;

        public StreamEntry()
        {
            m_vMessageTime = DateTime.UtcNow;
        }

        public virtual byte[] Encode()
        {
            var data = new List<byte>();

            data.AddInt32(GetStreamEntryType());
            data.AddInt32(0);
            data.AddInt32(m_vId);
            data.Add(3);
            data.AddInt64(m_vSenderId);
            data.AddInt64(m_vHomeId);
            data.AddString(m_vSenderName);
            data.AddInt32(m_vSenderLevel);
            data.AddInt32(m_vSenderLeagueId);
            data.AddInt32(m_vSenderRole);
            data.AddInt32(GetAgeSeconds());

            return data.ToArray();
        }

        public int GetAgeSeconds()
        {
            return (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds -
                   (int)m_vMessageTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public long GetHomeId()
        {
            return m_vHomeId;
        }

        public int GetId()
        {
            return m_vId;
        }

        public long GetSenderId()
        {
            return m_vSenderId;
        }

        public int GetSenderLeagueId()
        {
            return m_vSenderLeagueId;
        }

        public int GetSenderLevel()
        {
            return m_vSenderLevel;
        }

        public string GetSenderName()
        {
            return m_vSenderName;
        }

        public int GetSenderRole()
        {
            return m_vSenderRole;
        }

        public virtual int GetStreamEntryType()
        {
            return -1;
        }

        public virtual void Load(JObject jsonObject)
        {
            m_vId = jsonObject["id"].ToObject<int>();
            m_vSenderId = jsonObject["sender_id"].ToObject<long>();
            m_vHomeId = jsonObject["home_id"].ToObject<long>();
            m_vSenderLevel = jsonObject["sender_level"].ToObject<int>();
            m_vSenderName = jsonObject["sender_name"].ToObject<string>();
            m_vSenderLeagueId = jsonObject["sender_leagueId"].ToObject<int>();
            m_vSenderRole = jsonObject["sender_role"].ToObject<int>();
            m_vMessageTime = jsonObject["message_time"].ToObject<DateTime>();
        }

        public virtual JObject Save(JObject jsonObject)
        {
            jsonObject.Add("type", GetStreamEntryType());
            jsonObject.Add("id", m_vId);
            jsonObject.Add("sender_id", m_vSenderId);
            jsonObject.Add("home_id", m_vHomeId);
            jsonObject.Add("sender_level", m_vSenderLevel);
            jsonObject.Add("sender_name", m_vSenderName);
            jsonObject.Add("sender_leagueId", m_vSenderLeagueId);
            jsonObject.Add("sender_role", m_vSenderRole);
            jsonObject.Add("message_time", m_vMessageTime);

            return jsonObject;
        }

        public void SetAvatar(ClientAvatar avatar)
        {
            m_vSenderId = avatar.GetId();
            m_vHomeId = avatar.GetId();
            m_vSenderName = avatar.GetAvatarName();
            m_vSenderLeagueId = avatar.GetLeagueId();
            m_vSenderLevel = avatar.GetAvatarLevel();
        }

        public void SetHomeId(long id)
        {
            m_vHomeId = id;
        }

        public void SetId(int id)
        {
            m_vId = id;
        }

        public void SetSenderId(long id)
        {
            m_vSenderId = id;
        }

        public void SetSenderLeagueId(int leagueId)
        {
            m_vSenderLeagueId = leagueId;
        }

        public void SetSenderLevel(int level)
        {
            m_vSenderLevel = level;
        }

        public void SetSenderName(string name)
        {
            m_vSenderName = name;
        }

        public void SetSenderRole(int role)
        {
            m_vSenderRole = role;
        }
    }
}