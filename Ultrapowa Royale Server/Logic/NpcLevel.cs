namespace UCS.Logic
{
    internal class NpcLevel
    {
        private const int m_vType = 0x01036640;

        public NpcLevel()
        {
            //Deserialization
        }

        public NpcLevel(int index)
        {
            //this.Name = ObjectManager.NpcsData.GetData(index, 0).Name;
            Index = index;
            Stars = 0;
            LootedGold = 0;
            LootedElixir = 0;
        }

        public int Id
        {
            get { return m_vType + Index; }
        }

        public int Index { get; set; }
        public int LootedElixir { get; set; }
        public int LootedGold { get; set; }
        public string Name { get; set; }
        public int Stars { get; set; }
    }
}