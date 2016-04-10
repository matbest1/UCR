namespace UCS.GameFiles
{
    internal class VariablesData : Data
    {
        public VariablesData(CSVRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }
        public int Value { get; set; }
    }
}