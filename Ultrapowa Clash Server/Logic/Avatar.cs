using System;
using System.Collections.Generic;
using UCS.Core;
using UCS.GameFiles;

namespace UCS.Logic
{
    internal class Avatar
    {
        private int m_vCastleLevel;
        private int m_vCastleTotalCapacity;
        private int m_vCastleUsedCapacity;
        protected List<DataSlot> m_vHeroHealth;
        protected List<DataSlot> m_vHeroState;

        //a1 + 8
        protected List<DataSlot> m_vHeroUpgradeLevel;

        protected List<DataSlot> m_vResourceCaps;

        //private int m_vRemainingShieldTime;
        protected List<DataSlot> m_vResources;

        protected List<DataSlot> m_vSpellCount;
        protected List<DataSlot> m_vSpellUpgradeLevel;
        private int m_vTownHallLevel; //a1 + 88

        //a1 + 20
        //a1 + 12
        //a1 + 16
        protected List<DataSlot> m_vUnitCount;

        //a1 + 24
        //a1 + 28
        //a1 + 36
        //protected List<DataSlot> m_vNpcStars { get; set; }//a1 + 56
        //protected List<DataSlot> m_vLootedNpcGold { get; set; }//a1 + 60
        //protected List<DataSlot> m_vLootedNpcElixir { get; set; }//a1 + 64
        protected List<DataSlot> m_vUnitUpgradeLevel;

        //a1 + 40
        //a1 + 44

        public Avatar()
        {
            m_vResources = new List<DataSlot>();
            m_vResourceCaps = new List<DataSlot>();
            m_vUnitCount = new List<DataSlot>();
            m_vUnitUpgradeLevel = new List<DataSlot>();
            m_vHeroHealth = new List<DataSlot>();
            m_vHeroUpgradeLevel = new List<DataSlot>();
            m_vHeroState = new List<DataSlot>();
            m_vSpellCount = new List<DataSlot>();
            m_vSpellUpgradeLevel = new List<DataSlot>();
        }

        public void CommodityCountChangeHelper(int commodityType, Data data, int count)
        {
            if (data.GetDataType() == 2)
            {
                if (commodityType == 0)
                {
                    var resourceCount = GetResourceCount((ResourceData)data);
                    var newResourceValue = Math.Max(resourceCount + count, 0);
                    if (count >= 1)
                    {
                        var resourceCap = GetResourceCap((ResourceData)data);
                        if (resourceCount < resourceCap)
                        {
                            if (newResourceValue > resourceCap)
                            {
                                newResourceValue = resourceCap;
                            }
                        }
                    }
                    Debugger.WriteLine(string.Format("Old Resources: {0} New Resources: {1} Resource Cap: {2}", GetResourceCount((ResourceData)data), newResourceValue, GetResourceCap((ResourceData)data)), null, 5);
                    SetResourceCount((ResourceData)data, newResourceValue);
                }
            }
        }

        public int GetAllianceCastleLevel()
        {
            return m_vCastleLevel;
        }

        public int GetAllianceCastleTotalCapacity()
        {
            return m_vCastleTotalCapacity;
        }

        public int GetAllianceCastleUsedCapacity()
        {
            return m_vCastleUsedCapacity;
        }

        public static int GetDataIndex(List<DataSlot> dsl, Data d)
        {
            return dsl.FindIndex(ds => ds.Data == d);
        }

        public int GetResourceCap(ResourceData rd)
        {
            var index = GetDataIndex(m_vResourceCaps, rd);
            var count = 0;
            if (index != -1)
                count = m_vResourceCaps[index].Value;
            return count;
        }

        public List<DataSlot> GetResourceCaps()
        {
            return m_vResourceCaps;
        }

        public int GetResourceCount(ResourceData rd)
        {
            var index = GetDataIndex(m_vResources, rd);
            var count = 0;
            if (index != -1)
                count = m_vResources[index].Value;
            return count;
        }

        public List<DataSlot> GetResources()
        {
            return m_vResources;
        }

        public List<DataSlot> GetSpells()
        {
            return m_vSpellCount;
        }

        public int GetTownHallLevel()
        {
            return m_vTownHallLevel;
        }

        public int GetUnitCount(CombatItemData cd)
        {
            var result = 0;
            if (cd.GetCombatItemType() == 1)
            {
                var index = GetDataIndex(m_vSpellCount, cd);
                if (index != -1)
                    result = m_vSpellCount[index].Value;
            }
            else
            {
                var index = GetDataIndex(m_vUnitCount, cd);
                if (index != -1)
                    result = m_vUnitCount[index].Value;
            }
            return result;
        }

        public List<DataSlot> GetUnits()
        {
            return m_vUnitCount;
        }

        public int GetUnitUpgradeLevel(CombatItemData cd)
        {
            var result = 0;
            switch (cd.GetCombatItemType())
            {
                case 2:
                    {
                        var index = GetDataIndex(m_vHeroUpgradeLevel, cd);
                        if (index != -1)
                            result = m_vHeroUpgradeLevel[index].Value;
                        break;
                    }
                case 1:
                    {
                        var index = GetDataIndex(m_vSpellUpgradeLevel, cd);
                        if (index != -1)
                            result = m_vSpellUpgradeLevel[index].Value;
                        break;
                    }

                default:
                    {
                        var index = GetDataIndex(m_vUnitUpgradeLevel, cd);
                        if (index != -1)
                            result = m_vUnitUpgradeLevel[index].Value;
                        break;
                    }
            }
            return result;
        }

        public int GetUnusedResourceCap(ResourceData rd)
        {
            var resourceCount = GetResourceCount(rd);
            var resourceCap = GetResourceCap(rd);
            return Math.Max(resourceCap - resourceCount, 0);
        }

        public void SetAllianceCastleLevel(int level)
        {
            m_vCastleLevel = level;
        }

        public void SetAllianceCastleTotalCapacity(int totalCapacity)
        {
            m_vCastleTotalCapacity = totalCapacity;
        }

        public void SetAllianceCastleUsedCapacity(int usedCapacity)
        {
            m_vCastleUsedCapacity = usedCapacity;
        }

        public void SetHeroHealth(HeroData hd, int health)
        {
            var index = GetDataIndex(m_vHeroHealth, hd);
            if (index == -1)
            {
                var ds = new DataSlot(hd, health);
                m_vHeroHealth.Add(ds);
            }
            else
            {
                m_vHeroHealth[index].Value = health;
            }
        }

        public void SetHeroState(HeroData hd, int state)
        {
            var index = GetDataIndex(m_vHeroState, hd);
            if (index == -1)
            {
                var ds = new DataSlot(hd, state);
                m_vHeroState.Add(ds);
            }
            else
            {
                m_vHeroState[index].Value = state;
            }
        }

        public void SetResourceCap(ResourceData rd, int value)
        {
            var index = GetDataIndex(m_vResourceCaps, rd);
            if (index == -1)
            {
                var ds = new DataSlot(rd, value);
                m_vResourceCaps.Add(ds);
            }
            else
            {
                m_vResourceCaps[index].Value = value;
            }
        }

        public void SetResourceCount(ResourceData rd, int value)
        {
            var index = GetDataIndex(m_vResources, rd);
            if (index == -1)
            {
                var ds = new DataSlot(rd, value);
                m_vResources.Add(ds);
            }
            else
            {
                m_vResources[index].Value = value;
            }
            //LogicLevel::getComponentManager(v18);
            //LogicComponentManager::divideAvatarResourcesToStorages(v19)
        }

        public void SetTownHallLevel(int level)
        {
            m_vTownHallLevel = level;
        }

        public void SetUnitCount(CombatItemData cd, int count)
        {
            switch (cd.GetCombatItemType())
            {
                case 1:
                    {
                        var index = GetDataIndex(m_vSpellCount, cd);
                        if (index != -1)
                            m_vSpellCount[index].Value = count;
                        else
                        {
                            var ds = new DataSlot(cd, count);
                            m_vSpellCount.Add(ds);
                        }
                        break;
                    }
                default:
                    {
                        var index = GetDataIndex(m_vUnitCount, cd);
                        if (index != -1)
                            m_vUnitCount[index].Value = count;
                        else
                        {
                            var ds = new DataSlot(cd, count);
                            m_vUnitCount.Add(ds);
                        }
                        break;
                    }
            }
        }

        public void SetUnitUpgradeLevel(CombatItemData cd, int level)
        {
            switch (cd.GetCombatItemType())
            {
                case 2:
                    {
                        var index = GetDataIndex(m_vHeroUpgradeLevel, cd);
                        if (index != -1)
                            m_vHeroUpgradeLevel[index].Value = level;
                        else
                        {
                            var ds = new DataSlot(cd, level);
                            m_vHeroUpgradeLevel.Add(ds);
                        }
                        break;
                    }
                case 1:
                    {
                        var index = GetDataIndex(m_vSpellUpgradeLevel, cd);
                        if (index != -1)
                            m_vSpellUpgradeLevel[index].Value = level;
                        else
                        {
                            var ds = new DataSlot(cd, level);
                            m_vSpellUpgradeLevel.Add(ds);
                        }
                        break;
                    }
                default:
                    {
                        var index = GetDataIndex(m_vUnitUpgradeLevel, cd);
                        if (index != -1)
                            m_vUnitUpgradeLevel[index].Value = level;
                        else
                        {
                            var ds = new DataSlot(cd, level);
                            m_vUnitUpgradeLevel.Add(ds);
                        }
                        break;
                    }
            }
        }
    }
}