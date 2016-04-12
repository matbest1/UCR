namespace UCS.Logic
{
    internal class ComponentFilter : GameObjectFilter
    {
        public int Type; //a1 + 20

        public ComponentFilter(int type)
        {
            Type = type;
        }

        public override bool IsComponentFilter()
        {
            return true;
        }

        public bool TestComponent(Component c)
        {
            var go = c.GetParent();
            return TestGameObject(go);
        }

        public new bool TestGameObject(GameObject go)
        {
            var result = false;
            var c = go.GetComponent(Type, true);
            if (c != null)
            {
                result = base.TestGameObject(go);
            }
            return result;
        }
    }
}