namespace UCS.GameFiles
{
    internal class RegionsData : Data
    {
        public RegionsData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }
        public string TID { get; set; }
        public string DisplayName { get; set; }
        public bool IsCountry { get; set; }
    }
}