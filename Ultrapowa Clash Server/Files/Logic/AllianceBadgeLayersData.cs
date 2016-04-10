namespace UCS.GameFiles
{
    internal class AllianceBadgeLayersData : Data
    {
        public AllianceBadgeLayersData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string SWF { get; set; }
        public string ExportName { get; set; }
        public int RequiredClanLevel { get; set; }
    }
}