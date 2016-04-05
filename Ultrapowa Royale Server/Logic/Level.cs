using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using UCS.PacketProcessing;

namespace UCS.Logic
{
    internal class Level
    {
        private readonly ClientAvatar m_vClientAvatar;
        public GameObjectManager GameObjectManager;
        private byte m_vAccountPrivileges;
        private byte m_vAccountStatus;
        private string m_vIPAddress;
        private Client m_vClient;
        private DateTime m_vTime;

        public Level()
        {
            GameObjectManager = new GameObjectManager(this);
            m_vClientAvatar = new ClientAvatar();
            m_vAccountPrivileges = 0;
            m_vAccountStatus = 0;
        }

        public Level(long id)
        {
            GameObjectManager = new GameObjectManager(this);
            m_vClientAvatar = new ClientAvatar(id);
            m_vTime = DateTime.UtcNow;
            m_vAccountPrivileges = 0;
            m_vAccountStatus = 0;
        }

        public byte GetAccountPrivileges()
        {
            return m_vAccountPrivileges;
        }

        public string GetIPAddress()
        {
            return m_vIPAddress;
        }

        public void BanIP()
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"banned-ip.txt", true))
            {
                file.WriteLine(m_vIPAddress);
            }
        }

        public void DeBanIP()
        {
            var oldLines = System.IO.File.ReadAllLines("banned-ip.txt");
            var newLines = oldLines.Where(line => !line.Contains(m_vIPAddress));
            System.IO.File.WriteAllLines("banned-ip.txt", newLines);
        }

        public void SetIPAddress(string IP)
        {
            m_vIPAddress = IP;
        }

        public byte GetAccountStatus()
        {
            return m_vAccountStatus;
        }

        public Client GetClient()
        {
            return m_vClient;
        }

        public ClientAvatar GetHomeOwnerAvatar()
        {
            return m_vClientAvatar;
        }

        public ClientAvatar GetPlayerAvatar()
        {
            return m_vClientAvatar;
        }

        public DateTime GetTime()
        {
            return m_vTime;
        }

        public void LoadFromJSON(string jsonString)
        {
            var jsonObject = JObject.Parse(jsonString);
            GameObjectManager.Load(jsonObject);
        }

        public string SaveToJSON()
        {
            return JsonConvert.SerializeObject(GameObjectManager.Save());
        }

        public void SetAccountPrivileges(byte privileges)
        {
            m_vAccountPrivileges = privileges;
        }

        public void SetAccountStatus(byte status)
        {
            m_vAccountStatus = status;
        }

        public void SetClient(Client client)
        {
            m_vClient = client;
        }

        public void SetHome(string jsonHome)
        {
            GameObjectManager.Load(JObject.Parse(jsonHome));
        }

        public void SetTime(DateTime t)
        {
            m_vTime = t;
        }

        public void Tick()
        {
            SetTime(DateTime.UtcNow);
            GameObjectManager.Tick();
        }
    }
}