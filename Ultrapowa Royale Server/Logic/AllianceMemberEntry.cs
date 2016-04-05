using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using UCS.Core;
using UCS.Helpers;

namespace UCS.Logic
{
    internal class AllianceMemberEntry
    {
        private readonly int m_vDonatedTroops;
        private readonly byte m_vIsNewMember;
        private readonly int m_vReceivedTroops;
        private readonly int[] m_vRoleTable = { 1, 1, 4, 2, 3 };

        private readonly int m_vWarCooldown;

        private readonly int m_vWarOptInStatus;

        //private long m_vHomeId;
        private long m_vAvatarId;

        //mapping roles so comparison is easier
        //private int m_vExpLevel;
        //private int m_vScore;
        private int m_vOrder;

        private int m_vPreviousOrder;

        //private string m_vName;
        private int m_vRole; //1 : member, 2 : chef, 3 : aîné, 4 : chef adjoint

        public AllianceMemberEntry(long avatarId)
        {
            m_vAvatarId = avatarId;
            m_vIsNewMember = 0;
            m_vOrder = 1;
            m_vPreviousOrder = 1;
            m_vRole = 1;
            m_vDonatedTroops = 200;
            m_vReceivedTroops = 100;
            m_vWarCooldown = 0;
            m_vWarOptInStatus = 1;
        }

        public static void Decode(byte[] avatarData)
        {
            using (var br = new BinaryReader(new MemoryStream(avatarData)))
            {
            }
        }

        //00 00 00 2A 00 17 E8 BD
        //00 00 00 06
        //6B 61 69 73 65 72
        //00 00 00 02
        //00 00 00 58
        //00 00 00 00
        //00 00 0B 0C
        //00 00 00 83
        //00 00 00 5B
        //00 00 00 01
        //00 00 00 01
        //00
        //00 01 15 7A
        //00 00 00 01
        //01
        //00 00 00 2A 00 17 E8 BD

        public byte[] Encode()
        {
            var data = new List<byte>();

            var avatar = ResourcesManager.GetPlayer(m_vAvatarId);
            data.AddInt64(m_vAvatarId);
            data.AddString(avatar.GetPlayerAvatar().GetAvatarName());
            data.AddInt32(m_vRole);
            data.AddInt32(avatar.GetPlayerAvatar().GetAvatarLevel());
            data.AddInt32(avatar.GetPlayerAvatar().GetLeagueId());
            data.AddInt32(avatar.GetPlayerAvatar().GetScore());
            data.AddInt32(m_vDonatedTroops);
            data.AddInt32(m_vReceivedTroops);
            data.AddInt32(m_vOrder);
            data.AddInt32(m_vPreviousOrder);
            data.Add(m_vIsNewMember);
            data.AddInt32(m_vWarCooldown);
            data.AddInt32(m_vWarOptInStatus);
            data.Add(1);
            data.AddInt64(m_vAvatarId);

            return data.ToArray();
        }

        public long GetAvatarId()
        {
            return m_vAvatarId;
        }

        public static int GetDonations()
        {
            return 150;
        }

        /*public long GetHomeid()
        {
            return m_vHomeId;
        }

        public int GetExpLevel()
        {
            return m_vExpLevel;
        }

        public string GetName()
        {
            return m_vName;
        }*/

        public int GetOrder()
        {
            return m_vOrder;
        }

        public int GetPreviousOrder()
        {
            return m_vPreviousOrder;
        }

        public int GetRole()
        {
            return m_vRole;
        }

        /*public int GetScore()
        {
            return m_vScore;
        }*/

        public bool HasLowerRoleThan(int role)
        {
            var result = true;
            if (role < m_vRoleTable.Length && m_vRole < m_vRoleTable.Length)
            {
                if (m_vRoleTable[m_vRole] >= m_vRoleTable[role])
                    result = false;
            }
            return result;
        }

        public byte IsNewMember()
        {
            return m_vIsNewMember;
        }

        public void Load(JObject jsonObject)
        {
            m_vAvatarId = jsonObject["avatar_id"].ToObject<long>();
            m_vRole = jsonObject["role"].ToObject<int>();
        }

        public JObject Save(JObject jsonObject)
        {
            jsonObject.Add("avatar_id", m_vAvatarId);
            jsonObject.Add("role", m_vRole);
            return jsonObject;
        }

        public void SetAvatarId(long id)
        {
            m_vAvatarId = id;
        }

        /*public void SetExpLevel(int level)
        {
            m_vExpLevel = level;
        }

        public void SetHomeId(long id)
        {
            m_vHomeId = id;
        }

        public void SetName(string name)
        {
            m_vName = name;
        }*/

        public void SetOrder(int order)
        {
            m_vOrder = order;
        }

        public void SetPreviousOrder(int order)
        {
            m_vPreviousOrder = order;
        }

        public void SetRole(int role)
        {
            m_vRole = role;
        }

        /*public void SetScore(int score)
        {
            m_vScore = score;
        } */
    }
}