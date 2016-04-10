using UCS.Core;

namespace UCS.GameFiles
{
    internal class ObstacleData : Data
    {
        public ObstacleData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public int AppearancePeriodHours { get; set; }

        public int ClearCost { get; set; }

        public string ClearEffect { get; set; }

        public string ClearResource { get; set; }

        public int ClearTimeSeconds { get; set; }

        public string ExportName { get; set; }

        public string ExportNameBase { get; set; }

        public string ExportNameBaseNpc { get; set; }

        public int Height { get; set; }

        public bool IsTombstone { get; set; }

        public int LootCount { get; set; }

        public int LootMultiplierForVersion2 { get; set; }

        public string LootResource { get; set; }

        public int MinRespawnTimeHours { get; set; }

        public bool Passable { get; set; }

        public string PickUpEffect { get; set; }

        public string Resource { get; set; }

        public int RespawnWeight { get; set; }

        public string SWF { get; set; }

        public string TID { get; set; }

        public int TombGroup { get; set; }

        public int Width { get; set; }

        public ResourceData GetClearingResource()
        {
            return ObjectManager.DataTables.GetResourceByName(ClearResource);
        }
    }
}