namespace UCS.GameFiles
{
    internal class MissionsData : Data
    {
        public MissionsData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }
    }
}