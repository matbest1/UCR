using Newtonsoft.Json.Linq;
using UCS.Core;
using UCS.GameFiles;

namespace UCS.Logic
{
    internal class CombatComponent : Component
    {
        private const int m_vType = 0x01AB3F00;

        private int m_vAmmo;

        public CombatComponent(ConstructionItem ci, Level level) : base(ci)
        {
            var bd = (BuildingData)ci.GetData();
            if (bd.AmmoCount != 0)
            {
                m_vAmmo = bd.AmmoCount;
            }
        }

        public override int Type
        {
            get { return 1; }
        }

        public void FillAmmo()
        {
            var ca = GetParent().GetLevel().GetPlayerAvatar();
            var bd = (BuildingData)GetParent().GetData();
            var rd = ObjectManager.DataTables.GetResourceByName(bd.AmmoResource);

            if (ca.HasEnoughResources(rd, bd.AmmoCost))
            {
                ca.CommodityCountChangeHelper(0, rd, bd.AmmoCost);
                m_vAmmo = bd.AmmoCount;
            }
        }

        public override void Load(JObject jsonObject)
        {
            if (jsonObject["ammo"] != null)
            {
                m_vAmmo = jsonObject["ammo"].ToObject<int>();
            }
        }

        public override JObject Save(JObject jsonObject)
        {
            System.Console.WriteLine("hi");
            if (m_vAmmo != 0)
            {
                jsonObject.Add("ammo", m_vAmmo);
                System.Console.WriteLine("hi " + m_vAmmo);
            }
            return jsonObject;
        }
    }
}