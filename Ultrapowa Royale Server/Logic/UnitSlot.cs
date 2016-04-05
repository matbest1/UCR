using System.IO;
using UCS.GameFiles;
using UCS.Helpers;

namespace UCS.Logic
{
    internal class UnitSlot
    {
        public int Count;
        public int Level;
        public CombatItemData UnitData;

        public UnitSlot(CombatItemData cd, int level, int count)
        {
        }

        public void Decode(BinaryReader br)
        {
        }
    }
}