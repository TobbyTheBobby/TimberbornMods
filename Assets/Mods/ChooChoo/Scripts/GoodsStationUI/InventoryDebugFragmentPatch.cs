using System.Reflection;
using ChooChoo.GoodsStations;
using HarmonyLib;
using Timberborn.BaseComponentSystem;
using TobbyTools.UsedImplicitlySystem;

namespace ChooChoo.GoodsStationUI
{
    [UsedImplicitlyHarmonyPatch]
    public class InventoryDebugFragmentPatch
    {
        private static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("InventoryDebugFragment"), "ShowFragment", new[] { typeof(BaseComponent) });
        }

        private static bool Prefix(BaseComponent entity)
        {
            if (entity.TryGetComponentFast(out GoodsStation _))
            {
                return false;
            }

            return true;
        }
    }
}