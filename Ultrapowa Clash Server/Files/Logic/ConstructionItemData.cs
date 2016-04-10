namespace UCS.GameFiles
{
    internal class ConstructionItemData : Data
    {
        public ConstructionItemData(CSVRow row, DataTable dt) : base(row, dt)
        {
        }

        public virtual int GetBuildCost(int level)
        {
            return -1;
        }

        public virtual ResourceData GetBuildResource(int level)
        {
            return null;
        }

        public virtual int GetConstructionTime(int level)
        {
            return -1;
        }

        public virtual int GetRequiredTownHallLevel(int level)
        {
            return -1;
        }

        public virtual int GetUpgradeLevelCount()
        {
            return -1;
        }

        public virtual bool IsTownHall()
        {
            return false;
        }
    }
}