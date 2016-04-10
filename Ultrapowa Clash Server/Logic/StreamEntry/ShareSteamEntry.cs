using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UCS.Helpers;

namespace UCS.Logic
{
    internal class ShareStreamEntry : StreamEntry
    {
        public static int Unknown1 = 0;
        public static int Unknown2 = 0;
        public static int Unknown3 = 0;
        public static byte Unknown4 = 0;
        public static string Message = "Look this battle !";
        public static string EnemyName = "UltraPowa";
        public static string ReplayJson;
        public static int Unknown5 = 0;
        public static int Unknown6 = 0;
        public static int Unknown7 = 0;

        public override byte[] Encode()
        {
            var data = new List<byte>();
            data.AddRange(base.Encode());
            data.AddInt32(Unknown1);
            data.AddInt32(Unknown2);
            data.AddInt32(Unknown3);
            data.Add(Unknown4);
            data.AddString(Message);
            data.AddString(EnemyName);
            data.AddString(ReplayJson);
            data.AddInt32(Unknown5);
            data.AddInt32(Unknown6);
            data.AddInt32(Unknown7);
            return data.ToArray();
        }

        public override int GetStreamEntryType()
        {
            return 5;
        }

        public override void Load(JObject jsonObject)
        {
            base.Load(jsonObject);
            Unknown1 = jsonObject["unknown1"].ToObject<int>();
            Unknown2 = jsonObject["unknown2"].ToObject<int>();
            Unknown3 = jsonObject["unknown3"].ToObject<int>();
            Unknown4 = jsonObject["unknown4"].ToObject<byte>();
            Message = jsonObject["message"].ToObject<string>();
            EnemyName = jsonObject["enemy"].ToObject<string>();
            ReplayJson = jsonObject["replay"].ToObject<string>();
            Unknown5 = jsonObject["unknown5"].ToObject<int>();
            Unknown6 = jsonObject["unknown6"].ToObject<int>();
            Unknown7 = jsonObject["unknown7"].ToObject<int>();
        }

        public override JObject Save(JObject jsonObject)
        {
            jsonObject = base.Save(jsonObject);
            jsonObject.Add("unknown1", Unknown1);
            jsonObject.Add("unknown2", Unknown2);
            jsonObject.Add("unknown3", Unknown3);
            jsonObject.Add("unknown4", Unknown4);

            jsonObject.Add("message", Message);
            jsonObject.Add("enemy", EnemyName);
            jsonObject.Add("replay", ReplayJson);

            jsonObject.Add("unknown5", Unknown5);
            jsonObject.Add("unknown6", Unknown6);
            jsonObject.Add("unknown7", Unknown7);
            return jsonObject;
        }

        public void SetEnemyName(string name)
        {
            EnemyName = name;
        }

        public void SetMessage(string message)
        {
            Message = message;
        }

        public void SetReplayjson(string json)
        {
            ReplayJson = json;
        }
    }
}