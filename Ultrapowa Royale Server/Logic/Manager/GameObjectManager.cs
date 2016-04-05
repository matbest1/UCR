using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UCS.Core;
using UCS.GameFiles;

namespace UCS.Logic
{
    internal class GameObjectManager
    {
        private readonly ComponentManager m_vComponentManager;
        private readonly List<List<GameObject>> m_vGameObjects;
        private readonly List<int> m_vGameObjectsIndex;
        private readonly Level m_vLevel;

        public GameObjectManager(Level l)
        {
            m_vLevel = l;
            m_vGameObjects = new List<List<GameObject>>();
            m_vGameObjectsIndex = new List<int>();
            for (var i = 0; i < 7; i++)
            {
                m_vGameObjects.Add(new List<GameObject>());
                m_vGameObjectsIndex.Add(0);
            }
            m_vComponentManager = new ComponentManager(m_vLevel);
        }

        public void Load(JObject jsonObject)
        {
        }

        public void RemoveGameObject(GameObject go)
        {
        }

        public JObject Save()
        {
            var jsonData = new JObject();
            return jsonData;
        }

        public void Tick()
        {
        }
    }
}