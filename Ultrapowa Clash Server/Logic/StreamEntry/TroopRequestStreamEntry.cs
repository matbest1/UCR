using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UCS.Helpers;

namespace UCS.Logic
{
    internal class TroopRequestStreamEntry : StreamEntry
    {
        public static int Unknown1 = 0;
        public static int Unknown2 = 2;
        public static int Unknown3 = 0;
        public static int Unknown4 = 2;
        public static int Unknown5 = 0;
        public static string Message;
        public static DataSlot AllianceDonation;
        public static DataSlot UnitComponent;

        public override byte[] Encode()
        {
            var data = new List<byte>();
            data.AddRange(base.Encode());
            data.AddInt32(Unknown1);
            data.AddInt32(Unknown2);
            data.AddInt32(Unknown3);
            data.AddInt32(Unknown4);
            data.AddInt32(Unknown5);
            data.AddDataSlots(new List<DataSlot>());
            data.AddString(Message);
            data.AddDataSlots(new List<DataSlot>());
            return data.ToArray();
        }

        public override int GetStreamEntryType()
        {
            return 1;
        }

        public override void Load(JObject jsonObject)
        {
            base.Load(jsonObject);
            Unknown1 = jsonObject["unknown1"].ToObject<int>();
            Unknown2 = jsonObject["unknown2"].ToObject<int>();
            Unknown3 = jsonObject["unknown3"].ToObject<int>();
            Unknown4 = jsonObject["unknown4"].ToObject<int>();
            Unknown5 = jsonObject["unknown5"].ToObject<int>();
            //AllianceDonation = jsonObject["donations"].ToObject<DataSlot>();
            Message = jsonObject["message"].ToObject<string>();
            //jsonObject["tdonations"].ToObject<DataSlot>();
        }

        public override JObject Save(JObject jsonObject)
        {
            jsonObject = base.Save(jsonObject);
            jsonObject.Add("unknown1", Unknown1);
            jsonObject.Add("unknown2", Unknown2);
            jsonObject.Add("unknown3", Unknown3);
            jsonObject.Add("unknown4", Unknown4);
            jsonObject.Add("unknown5", Unknown5);
            jsonObject.Add("donations", new JArray() { 300000, 0 });
            jsonObject.Add("message", Message);
            jsonObject.Add("tdonations", new JArray());
            return jsonObject;
        }

        public void SetMessage(string msg)
        {
            Message = msg;
        }
    }
}