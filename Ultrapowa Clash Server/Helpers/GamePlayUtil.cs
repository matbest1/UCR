using System;
using UCS.Core;
using UCS.GameFiles;

namespace UCS.Helpers
{
    internal static class GamePlayUtil
    {
        public static int CalculateResourceCost(int sup, int inf, int supCost, int infCost, int amount)
        {
            return (int)Math.Round((supCost - infCost) * (long)(amount - inf) / (sup - inf * 1.0)) + infCost;
        }

        public static int CalculateSpeedUpCost(int sup, int inf, int supCost, int infCost, int amount)
        {
            return (int)Math.Round((supCost - infCost) * (long)(amount - inf) / (sup - inf * 1.0)) + infCost;
        }

        public static int GetResourceDiamondCost(int resourceCount, ResourceData resourceData)
        {
            var globals = ObjectManager.DataTables.GetGlobals();
            return Globals.GetResourceDiamondCost(resourceCount, resourceData);
        }

        public static int GetSpeedUpCost(int seconds)
        {
            var globals = ObjectManager.DataTables.GetGlobals();
            return Globals.GetSpeedUpCost(seconds);
        }
    }
}