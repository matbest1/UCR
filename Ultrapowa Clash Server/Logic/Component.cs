using Newtonsoft.Json.Linq;

namespace UCS.Logic
{
    internal class Component
    {
        private readonly GameObject m_vParentGameObject;
        private bool m_vIsEnabled; //a1 + 8

        public Component()
        {
            //do not modify
        }

        public Component(GameObject go)
        {
            m_vIsEnabled = true;
            m_vParentGameObject = go;
        }

        public virtual int Type
        {
            get { return -1; }
        }

        public GameObject GetParent()
        {
            return m_vParentGameObject;
        }

        public bool IsEnabled()
        {
            return m_vIsEnabled;
        }

        public virtual void Load(JObject jsonObject)
        {
        }

        public virtual JObject Save(JObject jsonObject)
        {
            return jsonObject;
        }

        public void SetEnabled(bool status)
        {
            m_vIsEnabled = status;
        }

        public virtual void Tick()
        {
        }
    }
}