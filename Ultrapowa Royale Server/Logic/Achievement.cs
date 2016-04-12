using UCS.Core;

namespace UCS.Logic
{
    internal class Achievement
    {
        private const int m_vType = 0x015EF3C0;

        public Achievement()
        {
        }

        public Achievement(int index)
        {
            //this.Name = ObjectManager.AchievementsData.GetData(index, 0).Name;
            Index = index;
            Unlocked = false;
            Value = 0;
        }

        public int Id
        {
            get { return m_vType + Index; }
        }

        public int Index { get; set; }
        public string Name { get; set; }
        public bool Unlocked { get; set; }
        public int Value { get; set; }
    }
}