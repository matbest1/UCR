using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UCS.Core;
using UCS.GameFiles;

namespace UCS.Logic
{
    internal class GameObjectManager
    {
        private readonly ComponentManager m_vComponentManager;
        private readonly List<GameObject> m_vGameObjectRemoveList;
        private readonly List<List<GameObject>> m_vGameObjects;
        private readonly List<int> m_vGameObjectsIndex;
        private readonly Level m_vLevel;
        private readonly ObstacleManager m_vObstacleManager;


        public GameObjectManager(Level l)
        {
            m_vLevel = l;
            m_vGameObjects = new List<List<GameObject>>();
            m_vGameObjectRemoveList = new List<GameObject>();
            m_vGameObjectsIndex = new List<int>();
            for (var i = 0; i < 7; i++)
            {
                m_vGameObjects.Add(new List<GameObject>());
                m_vGameObjectsIndex.Add(0);
            }
            m_vComponentManager = new ComponentManager(m_vLevel);
            m_vObstacleManager = new ObstacleManager(m_vLevel);
        }
        public List<List<GameObject>> GetAllGameObjects()
        {
            return m_vGameObjects;
        }
        public ObstacleManager GetObstacleManager()
        {
            return m_vObstacleManager;
        }

        public void AddGameObject(GameObject go)
        {
            go.GlobalId = GenerateGameObjectGlobalId(go);
            if (go.ClassId == 0)
            {
                var b = (Building)go;
                var bd = b.GetBuildingData();
                if (bd.IsWorkerBuilding())
                    m_vLevel.WorkerManager.IncreaseWorkerCount();
            }
            m_vGameObjects[go.ClassId].Add(go);
        }

        public ComponentManager GetComponentManager()
        {
            return m_vComponentManager;
        }

        public GameObject GetGameObjectByID(int id)
        {
            var classId = GlobalID.GetClassID(id) - 500;
            return m_vGameObjects[classId].Find(g => g.GlobalId == id);
        }

        public List<GameObject> GetGameObjects(int id)
        {
            return m_vGameObjects[id];
        }

        public void Load(JObject jsonObject)
        {
            // Load json
        }

        public void RemoveGameObject(GameObject go)
        {
            m_vGameObjects[go.ClassId].Remove(go);
            if (go.ClassId == 0)
            {
                var b = (Building)go;
                var bd = b.GetBuildingData();
                if (bd.IsWorkerBuilding())
                {
                    m_vLevel.WorkerManager.DecreaseWorkerCount();
                }
            }
            RemoveGameObjectReferences(go);
        }
        private void RemoveGameObjectTotally(GameObject go)
        {
            m_vGameObjects[go.ClassId].Remove(go);
            if (go.ClassId == 0)
            {
                var b = (Building)go;
                var bd = b.GetBuildingData();
                if (bd.IsWorkerBuilding())
                    m_vLevel.WorkerManager.DecreaseWorkerCount();
            }
            RemoveGameObjectReferences(go);
        }

        public void RemoveGameObjectReferences(GameObject go)
        {
            m_vComponentManager.RemoveGameObjectReferences(go);
        }

        public JObject Save()
        {
            var jsonData = new JObject();
            var jsonDecksArray = new JArray();
            // Load deck cards
            jsonData.Add("decks", jsonDecksArray);

            var jsonCollectionObject = new JObject();
            var jsonSpellsArray = new JArray();
            jsonCollectionObject.Add("spells", jsonSpellsArray);
            jsonCollectionObject.Add("next_serial", 1);
            jsonData.Add("collection", jsonCollectionObject);
            
            jsonData.Add("selected_deck", 0);
            jsonData.Add("locations", new JArray());
            jsonData.Add("gambleCounters", new JArray());

            jsonData.Add("chest_slots",4);

            var jsonFreeChest = new JObject();
            jsonFreeChest.Add("ticks", 288000);
            jsonFreeChest.Add("remaining", 0);
            jsonFreeChest.Add("timestamp", -1);
            jsonData.Add("free_chest_t", jsonFreeChest);

            jsonData.Add("star_chest_cooldown", true);
            return jsonData;
        }

        public void Tick()
        {
            m_vComponentManager.Tick();
            foreach (var l in m_vGameObjects)
            {
                foreach (var go in l)
                    go.Tick();
            }
            foreach (var g in new List<GameObject>(m_vGameObjectRemoveList))
            {
                RemoveGameObjectTotally(g);
                m_vGameObjectRemoveList.Remove(g);
            }
        }

        private int GenerateGameObjectGlobalId(GameObject go)
        {
            var index = m_vGameObjectsIndex[go.ClassId];
            m_vGameObjectsIndex[go.ClassId]++;
            return GlobalID.CreateGlobalID(go.ClassId + 500, index);
        }
    }
}