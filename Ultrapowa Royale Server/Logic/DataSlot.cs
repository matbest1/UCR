using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using UCS.Core;
using UCS.GameFiles;
using UCS.Helpers;

namespace UCS.Logic
{
    internal class DataSlot
    {
        public Data Data;
        public int Value;

        public DataSlot(Data d, int value)
        {
            Data = d;
            Value = value;
        }

        public void Decode(BinaryReader br)
        {
            Data = br.ReadDataReference();
            Value = br.ReadInt32WithEndian();
        }

        public byte[] Encode()
        {
            var data = new List<byte>();
            data.AddInt32(Data.GetGlobalID());
            data.AddInt32(Value);
            return data.ToArray();
        }

        public void Load(JObject jsonObject)
        {
            Data = ObjectManager.DataTables.GetDataById(jsonObject["global_id"].ToObject<int>());
            Value = jsonObject["value"].ToObject<int>();
        }

        public JObject Save(JObject jsonObject)
        {
            jsonObject.Add("global_id", Data.GetGlobalID());
            jsonObject.Add("value", Value);
            return jsonObject;
        }
    }
}