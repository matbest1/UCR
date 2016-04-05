using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Windows;
using UCS.GameFiles;

namespace UCS.Logic
{
    internal class GameObject
    {
        private readonly List<Component> m_vComponents;
        private readonly Data m_vData;
        private readonly Level m_vLevel;

        public GameObject(Data data, Level level)
        {
            m_vLevel = level;
            m_vData = data;
            m_vComponents = new List<Component>();
            for (var i = 0; i < 11; i++)
                m_vComponents.Add(new Component());
        }

        public virtual int ClassId
        {
            get { return -1; }
        }

        public int GlobalId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public void AddComponent(Component c)
        {
        }

        public Component GetComponent(int index, bool test)
        {
            Component result = null;
            if (!test || m_vComponents[index].IsEnabled())
                result = m_vComponents[index];
            return result;
        }

        public Data GetData()
        {
            return m_vData;
        }

        public Level GetLevel()
        {
            return m_vLevel;
        }
    }
}