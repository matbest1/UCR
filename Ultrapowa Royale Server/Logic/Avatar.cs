using System;
using System.Collections.Generic;
using UCS.Core;
using UCS.GameFiles;

namespace UCS.Logic
{
    internal class Avatar
    {
        protected List<DataSlot> m_vResourceCaps;
        private int m_vRemainingShieldTime;
        protected List<DataSlot> m_vResources;

        public Avatar()
        {
            m_vResources = new List<DataSlot>();
            m_vResourceCaps = new List<DataSlot>();
        }

        public static int GetDataIndex(List<DataSlot> dsl, Data d)
        {
            return dsl.FindIndex(ds => ds.Data == d);
        }

        public List<DataSlot> GetResourceCaps()
        {
            return m_vResourceCaps;
        }

        public List<DataSlot> GetResources()
        {
            return m_vResources;
        }
    }
}