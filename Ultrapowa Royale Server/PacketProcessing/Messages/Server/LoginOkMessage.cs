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
            var pack = new List<byte>();
            
            pack.AddInt64(m_vAccountId);
            pack.AddInt64(m_vAccountId);
            pack.AddString(null);
            pack.AddString(m_vPassToken);
            pack.AddInt32(0);

            // DEBUG INFO
            Console.WriteLine("Account ID : " + m_vAccountId);
            Console.WriteLine("User Token : " + m_vPassToken);
            Console.WriteLine("FacebookID : " + m_vFacebookId);
            Console.WriteLine("GameCenter : " + m_vGamecenterId);
            Console.WriteLine("MajorVers  : " + m_vServerMajorVersion);
            Console.WriteLine("MinorVers  : " + m_vServerBuild);
            Console.WriteLine("RevisionV  : " + m_vContentVersion);
            Console.WriteLine("LoginCount : " + m_vSessionCount);
            Console.WriteLine("PlayTime S : " + m_vPlayTimeSeconds);
            Console.WriteLine("FB APP ID  : " + m_vFacebookAppID.ToString());
            Console.WriteLine("Cooldown S : " + m_vStartupCooldownSeconds.ToString());
            Console.WriteLine("CreateDate : " + m_vAccountCreatedDate);
            Console.WriteLine("Google ID  : " + m_vGoogleID);
            Console.WriteLine("CountryCod : " + m_vCountryCode);
            Console.WriteLine("Environne  : " + m_vServerEnvironment);
            // END DEBUG 

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
    }
}