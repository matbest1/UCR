using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using UCS.Core;
using UCS.GameFiles;
using UCS.Helpers;

namespace UCS.Logic
{
    internal class ClientAvatar : Avatar
    {
        private long m_vAllianceId;
        private int m_vAvatarLevel;
        private string m_vAvatarName;
        private int m_vCurrentGems;
        private long m_vCurrentHomeId;
        private int m_vExperience;
        private long m_vId;
        private byte m_vNameChangingLeft;
        private byte m_vnameChosenByUser;
        private int m_vLeagueId;
        private int m_vScore;

        public ClientAvatar()
        {
        }

        public ClientAvatar(long id) : this()
        {
        }

        public byte[] Encode()
        {
            var data = new List<byte>();
            return data.ToArray();
        }

        public void AddDiamonds(int diamondCount)
        {
            this.m_vCurrentGems += diamondCount;
        }

        public void AddExperience(int exp)
        {
        }

        public long GetAllianceId()
        {
            return m_vAllianceId;
        }

        public int GetAvatarLevel()
        {
            return m_vAvatarLevel;
        }

        public string GetAvatarName()
        {
            return m_vAvatarName;
        }

        public long GetCurrentHomeId()
        {
            return m_vCurrentHomeId;
        }

        public int GetDiamonds()
        {
            return m_vCurrentGems;
        }

        public long GetId()
        {
            return m_vId;
        }

        public int GetLeagueId()
        {
            return m_vLeagueId;
        }

        public int GetScore()
        {
            return m_vScore;
        }

        public bool HasEnoughDiamonds(int diamondCount)
        {
            return m_vCurrentGems >= diamondCount;
        }

        public void LoadFromJSON(string jsonString)
        {
        }

        public string SaveToJSON()
        {
            var jsonData = new JObject();
            return JsonConvert.SerializeObject(jsonData);
        }

        public void SetAllianceId(long id)
        {
            m_vAllianceId = id;
        }

        public void SetDiamonds(int count)
        {
            m_vCurrentGems = count;
        }

        public void SetLeagueId(int id)
        {
            m_vLeagueId = id;
        }

        public void SetScore(int newScore)
        {
            m_vScore = newScore;
            updateLeague();
        }

        public void SetName(string name)
        {
            m_vAvatarName = name;
            m_vnameChosenByUser = 0x01;
            m_vNameChangingLeft = 0x01;
        }

        public void UseDiamonds(int diamondCount)
        {
            m_vCurrentGems -= diamondCount;
        }

        private void updateLeague()
        {
        }
    }
}