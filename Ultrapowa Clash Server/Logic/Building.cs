using UCS.Core;
using UCS.GameFiles;

namespace UCS.Logic
{
    internal class Building : ConstructionItem
    {
        public Building(Data data, Level level) : base(data, level)
        {
            Locked = GetBuildingData().Locked;
            AddComponent(new HitpointComponent());
            if (GetBuildingData().IsHeroBarrack)
            {
                var hd = ObjectManager.DataTables.GetHeroByName(GetBuildingData().HeroType);
                AddComponent(new HeroBaseComponent(this, hd));
            }
            if (GetBuildingData().UpgradesUnits)
                AddComponent(new UnitUpgradeComponent(this));
            if (GetBuildingData().UnitProduction[0] > 0)
                AddComponent(new UnitProductionComponent(this));
            if (GetBuildingData().HousingSpace[0] > 0)
            {
                if (GetBuildingData().Bunker)
                    AddComponent(new BunkerComponent());
                else
                    AddComponent(new UnitStorageComponent(this, 0));
            }
            if (GetBuildingData().Damage[0] > 0)
                AddComponent(new CombatComponent(this, level));
            if (GetBuildingData().ProducesResource != null && GetBuildingData().ProducesResource != string.Empty)
            {
                var s = GetBuildingData().ProducesResource;
                AddComponent(new ResourceProductionComponent(this, level));
            }
            if (GetBuildingData().MaxStoredGold[0] > 0 ||
                GetBuildingData().MaxStoredElixir[0] > 0 ||
                GetBuildingData().MaxStoredDarkElixir[0] > 0 ||
                GetBuildingData().MaxStoredWarGold[0] > 0 ||
                GetBuildingData().MaxStoredWarElixir[0] > 0 ||
                GetBuildingData().MaxStoredWarDarkElixir[0] > 0)
                AddComponent(new ResourceStorageComponent(this));
        }

        public override int ClassId
        {
            get { return 0; }
        }

        public BuildingData GetBuildingData()
        {
            return (BuildingData)GetData();
        }
    }
}