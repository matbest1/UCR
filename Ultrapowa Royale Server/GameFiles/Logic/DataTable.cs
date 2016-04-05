using System.Collections.Generic;
using UCS.Logic;

namespace UCS.GameFiles
{
    internal class DataTable
    {
        protected List<Data> m_vData;
        protected int m_vIndex;

        public DataTable()
        {
            m_vIndex = 0;
            m_vData = new List<Data>();
        }

        public DataTable(CSVTable table, int index)
        {
            m_vIndex = index;
            m_vData = new List<Data>();

            for (var i = 0; i < table.GetRowCount(); i++)
            {
                var row = table.GetRowAt(i);
                var data = CreateItem(row);
                m_vData.Add(data);
            }
        }

        public Data CreateItem(CSVRow row)
        {
            var d = new Data(row, this);
            switch (m_vIndex)
            {
                case 13:
                    d = new GlobalData(row, this);
                    break;

                case 23:
                    d = new Data(row, this);
                    break;

                case 24:
                    d = new Data(row, this);
                    break;

                default:
                    break;
            }
            return d;
        }

        public Data GetDataByName(string name)
        {
            return m_vData.Find(d => d.GetName() == name);
        }

        public Data GetItemAt(int index)
        {
            return m_vData[index];
        }

        public Data GetItemById(int id)
        {
            var instanceId = GlobalID.GetInstanceID(id);
            return m_vData[instanceId];
        }

        public int GetItemCount()
        {
            return m_vData.Count;
        }

        public int GetTableIndex()
        {
            return m_vIndex;
        }
    }
}