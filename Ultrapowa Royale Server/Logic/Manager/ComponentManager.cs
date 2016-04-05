using System.Collections.Generic;
using System.Windows;
using UCS.Core;
using UCS.GameFiles;

namespace UCS.Logic
{
    internal class ComponentManager
    {
        private readonly List<List<Component>> m_vComponents;
        private readonly Level m_vLevel;

        public ComponentManager(Level l)
        {
            m_vComponents = new List<List<Component>>();
            for (var i = 0; i <= 10; i++)
                m_vComponents.Add(new List<Component>());
            m_vLevel = l;
        }

        public void AddComponent(Component c)
        {
            m_vComponents[c.Type].Add(c);
        }

        public static void Tick()
        {
        }
    }
}