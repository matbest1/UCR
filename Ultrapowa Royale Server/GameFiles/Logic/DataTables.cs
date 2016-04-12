using System.Collections.Generic;
using UCS.Logic;

namespace UCS.GameFiles
{
    internal class DataTables
    {
        private readonly List<DataTable> m_vDataTables;

        public DataTables()
        {
            m_vDataTables = new List<DataTable>();
            for (var i = 0; i < 41; i++)
                m_vDataTables.Add(new DataTable());
        }

        public Data GetDataById(int id)
        {
            var classId = GlobalID.GetClassID(id) - 1;
            var dt = m_vDataTables[classId];
            return dt.GetItemById(id);
        }

        public Globals GetGlobals()
        {
            return (Globals)m_vDataTables[13];
        }

        public DataTable GetTable(int i)
        {
            return m_vDataTables[i];
        }

        public void InitDataTable(CSVTable t, int index)
        {
            if (index == 13)
                m_vDataTables[index] = new Globals(t, index);
            else
                m_vDataTables[index] = new DataTable(t, index);
        }
    }
}