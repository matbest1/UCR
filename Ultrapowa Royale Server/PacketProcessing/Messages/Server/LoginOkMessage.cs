using Sodium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Packet 20104
    internal class LoginOkMessage : Message
    {
        private string m_vAccountCreatedDate;
        private long m_vAccountId;
        private int m_vContentVersion;
        private string m_vCountryCode;
        private int m_vDaysSinceStartedPlaying;
        private string m_vFacebookId;
        private string m_vGamecenterId;
        private string m_vPassToken;
        private int m_vPlayTimeSeconds;
        private int m_vServerBuild;
        private string m_vServerEnvironment;
        private int m_vServerMajorVersion;
        private string m_vServerTime;
        private int m_vSessionCount = 0;
        private int m_vStartupCooldownSeconds;
        private string m_vFacebookAppID = "297484437009394";
        private int m_vLastUpdate;
        private int m_vGoogleID;

        public LoginOkMessage(Client client) : base(client)
        {
            SetMessageType(20104);
        }

        public string Unknown11 { get; set; }

        public string Unknown9 { get; set; }

        public override void Encode()
        {
            List<byte> pack = new List<byte>();
            pack.AddInt64(m_vAccountId);
            pack.AddInt64(m_vAccountId);
            pack.AddString(m_vPassToken);
            pack.AddString(m_vFacebookId);
            pack.AddString(m_vGamecenterId);
            pack.AddInt32(m_vServerMajorVersion);
            pack.AddInt32(m_vServerBuild);
            pack.AddInt32(m_vContentVersion);
            pack.AddString(m_vServerEnvironment);
            pack.AddInt32(m_vSessionCount);
            pack.AddInt32(m_vPlayTimeSeconds);
            pack.AddInt32(0);
            pack.AddString(m_vFacebookAppID);
            pack.AddString((m_vStartupCooldownSeconds.ToString()));
            pack.AddString(m_vAccountCreatedDate);
            pack.AddInt32(0);
            pack.AddString(m_vGoogleID.ToString());
            pack.AddString(null);
            pack.AddString(m_vCountryCode);
            pack.AddString("someid2");
            Encrypt(pack.ToArray());
        }

        public void SetAccountCreatedDate(string date)
        {
            m_vAccountCreatedDate = date;
        }

        public void SetAccountId(long id)
        {
            m_vAccountId = id;
        }

        public void SetContentVersion(int version)
        {
            m_vContentVersion = version;
        }

        public void SetCountryCode(string code)
        {
            m_vCountryCode = code;
        }

        public void SetDaysSinceStartedPlaying(int days)
        {
            m_vDaysSinceStartedPlaying = days;
        }

        public void SetFacebookId(string id)
        {
            m_vFacebookId = id;
        }

        public void SetGamecenterId(string id)
        {
            m_vGamecenterId = id;
        }

        public void SetPassToken(string token)
        {
            m_vPassToken = token;
        }

        public void SetPlayTimeSeconds(int seconds)
        {
            m_vPlayTimeSeconds = seconds;
        }

        public void SetServerBuild(int build)
        {
            m_vServerBuild = build;
        }

        public void SetServerEnvironment(string env)
        {
            m_vServerEnvironment = env;
        }

        public void SetServerMajorVersion(int version)
        {
            m_vServerMajorVersion = version;
        }

        public void SetServerTime(string time)
        {
            m_vServerTime = time;
        }

        public void SetSessionCount(int count)
        {
            m_vSessionCount = count;
        }

        public void SetStartupCooldownSeconds(int seconds)
        {
            m_vStartupCooldownSeconds = seconds;
        }

        /*
        int LoginOk()
        {
            sub_103594(&unk_3C8E88, "googleId");
            sub_103594(&unk_3C8E48, "facebookId");
            sub_103594(&unk_3C8F90, "gamecenterId");
            sub_103594(&unk_3C90BC, "accountId");
            sub_103594(&unk_3C8EB0, "expLevel");
            sub_103594(&unk_3C8CCC, "gold");
            sub_103594(&unk_3C9080, "PCBalance");
            sub_103594(&unk_3C8ED8, "FreePCBalance");
            sub_103594(&unk_3C91C0, "allianceId");
            sub_103594(&unk_3C8DD0, "score");
            sub_103594(&unk_3C9044, "Session Count");
            sub_103594(&unk_3C91EC, "days_since_start");
            sub_103594(&unk_3C901C, "Total Minutes Played");
            sub_103594(&unk_3C8E34, "accountCreatedDate");
            sub_103594(&unk_3C90F8, "serverTime");
            sub_103594(&unk_3C8E0C, "device");
            sub_103594(&unk_3C8D08, "gamelang");
            sub_103594(&unk_3C8D30, "Level");
            sub_103594(&unk_3C8F54, "Tutorial");
            sub_103594(&unk_3C8FA4, "Game");
            sub_103594(&unk_3C9134, "Economy");
            sub_103594(&unk_3C8CE0, "Social");
            sub_103594(&unk_3C906C, "started");
            sub_103594(&unk_3C91AC, "cancelled");
            sub_103594(&unk_3C8C7C, "finished");
            sub_103594(&unk_3C8F40, "chat");
            sub_103594(&unk_3C9058, "membership");
            sub_103594(&unk_3C910C, "mail");
            sub_103594(&unk_3C8F7C, "UI");
            sub_103594(&unk_3C90A8, "SpendPC");
            sub_103594(&unk_3C8E5C, "GainFreePC");
            sub_103594(&unk_3C8F2C, "SpendGold");
            sub_103594(&unk_3C8D80, "Quest");
            sub_103594(&unk_3C9008, "SwitchAccount");
            sub_103594(&unk_3C8EC4, "FB connect");
            sub_103594(&unk_3C8DF8, "GameCenter connect");
            sub_103594(&unk_3C8F18, "InAppPurchase");
            sub_103594(&unk_3C8F68, "Alliance");
            sub_103594(&unk_3C8CA4, "World Chat");
            sub_103594(&unk_3C8D6C, "nature");
            sub_103594(&unk_3C8E70, "Achievement");
            sub_103594(&unk_3C8C54, "Sell Deco");
            sub_103594(&unk_3C8C68, "Spend PC");
            sub_103594(&unk_3C8CB8, "Gain Free PC");
            sub_103594(&unk_3C8FCC, "Battle");
            sub_103594(&unk_3C90E4, "Visiting players");
            sub_103594(&unk_3C915C, "FB connect");
            sub_103594(&unk_3C8FE0, "GameCenter connect");
            sub_103594(&unk_3C9030, "Building");
            sub_103594(&unk_3C8CF4, "Upgrade");
            sub_103594(&unk_3C8DBC, "Level reached");
            sub_103594(&unk_3C8DA8, "attackEnd");
            sub_103594(&unk_3C8E20, "npcAttackEnd");
            sub_103594(&unk_3C8D44, "Switch Account");
            sub_103594(&unk_3C9094, "Economy");
            sub_103594(&unk_3C8D58, "DeviceLink");
            sub_103594(&unk_3C9148, "ConnectionInterface");
            sub_103594(&unk_3C8D94, "globalId");
            sub_103594(&unk_3C9120, "upgradeLvl");
            sub_103594(&unk_3C8E9C, "enemyThLvl");
            sub_103594(&unk_3C90D0, "elixirLoot");
            sub_103594(&unk_3C8D1C, "goldLoot");
            sub_103594(&unk_3C9184, "darkElixirLoot");
            sub_103594(&unk_3C8F04, "scoreChange");
            sub_103594(&unk_3C8C90, "enemyScore");
            sub_103594(&unk_3C8DE4, "availableScore");
            sub_103594(&unk_3C9170, "stars");
            sub_103594(&unk_3C9198, "favoriteTroop");
            sub_103594(&unk_3C8FB8, "favoriteSpell");
            sub_103594(&unk_3C91D4, "npcId");
            sub_103594(&unk_3C8EEC, "PCChange");
            sub_103594(&unk_3C8FF4, "FreePCChange");
        }
        */
    }
}