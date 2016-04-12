using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using UCS.Core;
using UCS.GameFiles;
using UCS.Logic;

namespace UCS.Helpers
{
    internal static class Helpers
    {
        public static void AddDataSlots(this List<byte> list, List<DataSlot> data)
        {
            list.AddInt32(data.Count);
            foreach (var dataSlot in data)
            {
                list.AddRange(dataSlot.Encode());
            }
        }

        public static void AddInt32(this List<byte> list, int data)
        {
            list.AddRange(BitConverter.GetBytes(data).Reverse());
        }

        public static void AddInt64(this List<byte> list, long data)
        {
            list.AddRange(BitConverter.GetBytes(data).Reverse());
        }

        public static void AddString(this List<byte> list, string data)
        {
            if (data == null)
                list.AddRange(BitConverter.GetBytes(-1).Reverse());
            else
            {
                list.AddRange(BitConverter.GetBytes(Encoding.UTF8.GetByteCount(data)).Reverse());
                list.AddRange(Encoding.UTF8.GetBytes(data));
            }
        }

        public static byte[] ReadAllBytes(this BinaryReader br)
        {
            const int bufferSize = 4096;
            using (var ms = new MemoryStream())
            {
                var buffer = new byte[bufferSize];
                int count;
                while ((count = br.Read(buffer, 0, buffer.Length)) != 0)
                    ms.Write(buffer, 0, count);
                return ms.ToArray();
            }
        }

        public static Data ReadDataReference(this BinaryReader br)
        {
            var id = br.ReadInt32WithEndian();
            return ObjectManager.DataTables.GetDataById(id);
        }

        public static int ReadInt32WithEndian(this BinaryReader br)
        {
            var a32 = br.ReadBytes(4);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(a32);
            return BitConverter.ToInt32(a32, 0);
        }

        public static long ReadInt64WithEndian(this BinaryReader br)
        {
            var a64 = br.ReadBytes(8);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(a64);
            return BitConverter.ToInt64(a64, 0);
        }

        public static string ReadScString(this BinaryReader br)
        {
            var stringLength = br.ReadInt32WithEndian();
            string result;

            if (stringLength > -1)
            {
                if (stringLength > 0)
                {
                    var astr = br.ReadBytes(stringLength);
                    result = Encoding.UTF8.GetString(astr);
                }
                else
                {
                    result = string.Empty;
                }
            }
            else
                result = null;
            return result;
        }

        public static ushort ReadUInt16WithEndian(this BinaryReader br)
        {
            var a16 = br.ReadBytes(2);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(a16);
            return BitConverter.ToUInt16(a16, 0);
        }

        public static uint ReadUInt32WithEndian(this BinaryReader br)
        {
            var a32 = br.ReadBytes(4);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(a32);
            return BitConverter.ToUInt32(a32, 0);
        }

        public static bool TryRemove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> self, TKey key)
        {
            TValue ignored;
            return self.TryRemove(key, out ignored);
        }

        public static int ParseConfigInt(string str)
        {
            return int.Parse(ConfigurationManager.AppSettings[str]);
        }

        public static string parseConfigString(string str)
        {
            return ConfigurationManager.AppSettings[str];
        }
    }
}