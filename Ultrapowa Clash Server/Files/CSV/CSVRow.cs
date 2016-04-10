namespace UCS.GameFiles
{
    internal class CSVRow
    {
        private readonly CSVTable m_vCSVTable;
        private readonly int m_vRowStart;

        public CSVRow(CSVTable table)
        {
            m_vCSVTable = table;
            m_vRowStart = m_vCSVTable.GetColumnRowCount();
            m_vCSVTable.AddRow(this);
        }

        public int GetArraySize(string name)
        {
            var columnIndex = m_vCSVTable.GetColumnIndexByName(name);
            var result = 0;
            if (columnIndex != -1)
                result = m_vCSVTable.GetArraySizeAt(this, columnIndex);
            return result;
        }

        public string GetName()
        {
            return m_vCSVTable.GetValueAt(0, m_vRowStart);
        }

        public int GetRowOffset()
        {
            return m_vRowStart;
        }

        public string GetValue(string name, int level)
        {
            return m_vCSVTable.GetValue(name, level + m_vRowStart);
        }
    }
}