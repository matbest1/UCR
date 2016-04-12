using Sodium;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.HashFunction;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UCS.Core;
using UCS.Helpers;
using UCS.Logic;
using UCS.Network;
using UCS.PacketProcessing;

namespace UCS.PacketProcessing
{
    //Packet 10101
    internal class LoginMessage : Message
    {
        public long UserID;
        public string UserToken;
        public int Unknown;
        public string MasterHash;
        public string Unknown1;
        public string OpenUDID;
        public string MacAddress;
        public string DeviceModel;
        public string AdvertisingGUID;
        public string OSVersion;
        public byte Unknown2;
        public string Unknown3;
        public string AndroidDeviceID;
        public string Language;

        public LoginMessage(Client client, BinaryReader br) : base(client, br)
        {
            Decrypt();
        }

        public override void Decode()
        {
            if (Client.CState == 1)
            {
                using (var reader = new CoCSharpPacketReader(new MemoryStream(GetData())))
                {
                    UserID = reader.ReadInt64();
                    Console.WriteLine("UserID -> " + UserID);
                    UserToken = reader.ReadString();

                    Unknown = reader.ReadInt32();
                    Console.WriteLine("Unknown -> " + Unknown);

                    MasterHash = reader.ReadString();
                    Console.WriteLine("MasterHash -> " + MasterHash);
                    Unknown1 = reader.ReadString();
                    Console.WriteLine("Unknown1 -> " + Unknown1);
                    OpenUDID = reader.ReadString();
                    Console.WriteLine("OpenUDID -> " + OpenUDID);
                    MacAddress = reader.ReadString();
                    Console.WriteLine("MacAddress -> " + MacAddress);
                    DeviceModel = reader.ReadString();
                    Console.WriteLine("DeviceModel -> " + DeviceModel);

                    AdvertisingGUID = reader.ReadString();
                    Console.WriteLine("AdvertisingGUID -> " + AdvertisingGUID);
                    OSVersion = reader.ReadString();
                    Console.WriteLine("OSVersion -> " + OSVersion);
                    Unknown2 = reader.ReadByte();
                    Console.WriteLine("Unknown2 -> " + Unknown2);
                    Unknown3 = reader.ReadString();
                    Console.WriteLine("Unknown3 -> " + Unknown3);
                    AndroidDeviceID = reader.ReadString();
                    Console.WriteLine("AndroidDeviceID -> " + AndroidDeviceID);
                    Language = reader.ReadString();
                    Console.WriteLine("Language -> " + Language);
                }
            }
        }

        public override void Process(Level level)
        {
            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["maintenanceMode"]) || Client.CState == 0)
            {
                var p = new LoginFailedMessage(Client);
                p.SetErrorCode(10);
                PacketManager.ProcessOutgoingPacket(p);
                return;
            }

            /*
            var versionData = ConfigurationManager.AppSettings["clientVersion"].Split('.');
            if (versionData.Length >= 2)
            {
                var cv = ClientVersion.Split('.');
                if (cv[0] != versionData[0] || cv[1] != versionData[1])
                {
                    var p = new LoginFailedMessage(Client);
                    p.SetErrorCode(8);
                    p.SetUpdateURL(Convert.ToString(ConfigurationManager.AppSettings["UpdateUrl"]));
                    PacketManager.ProcessOutgoingPacket(p);
                    return;
                }
            }
            else
            {
                Debugger.WriteLine("[UCR][10101] Connection failed. UCR config key clientVersion is not properly set.");
            }
            */

            level = ResourcesManager.GetPlayer(UserID);
            if (level != null)
            {
                if (level.GetAccountStatus() == 99)
                {
                    var p = new LoginFailedMessage(Client);
                    p.SetErrorCode(11);
                    PacketManager.ProcessOutgoingPacket(p);
                    return;
                }
            }
            else
            {
                level = ObjectManager.CreateAvatar(UserID);
                var tokenSeed = new byte[20];
                new Random().NextBytes(tokenSeed);
                using (SHA1 sha = new SHA1CryptoServiceProvider())
                    UserToken = BitConverter.ToString(sha.ComputeHash(tokenSeed)).Replace("-", string.Empty);
            }

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["useCustomPatch"]))
                if (MasterHash != ObjectManager.FingerPrint.sha)
                {
                    var p = new LoginFailedMessage(Client);
                    p.SetErrorCode(7);
                    p.SetResourceFingerprintData(ObjectManager.FingerPrint.SaveToJson());
                    p.SetContentURL(ConfigurationManager.AppSettings["patchingServer"]);
                    p.SetUpdateURL("http://www.ultrapowa.com/client");
                    PacketManager.ProcessOutgoingPacket(p);
                    return;
                }

            Client.ClientSeed = Unknown;
            ResourcesManager.LogPlayerIn(level, Client);
            level.Tick();

            
            var loginOk = new LoginOkMessage(Client);
            var avatar = level.GetPlayerAvatar();
            loginOk.SetAccountId(avatar.GetId());
            loginOk.SetPassToken(UserToken);
            loginOk.SetServerEnvironment("prod");
            loginOk.SetDaysSinceStartedPlaying(10);
            loginOk.SetServerTime(Math.Round(level.GetTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds * 1000).ToString());
            loginOk.SetAccountCreatedDate("1414003838000");
            loginOk.SetStartupCooldownSeconds(0);
            loginOk.SetCountryCode(Language);
            PacketManager.ProcessOutgoingPacket(loginOk);
            

            /*
            var alliance = ObjectManager.GetAlliance(level.GetPlayerAvatar().GetAllianceId());
            if (alliance == null)
                level.GetPlayerAvatar().SetAllianceId(0);

            PacketManager.ProcessOutgoingPacket(new OwnHomeDataMessage(Client, level));
            */
        }
    }
}