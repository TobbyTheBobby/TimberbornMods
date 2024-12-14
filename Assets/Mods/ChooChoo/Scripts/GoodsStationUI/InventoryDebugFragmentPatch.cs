using System.Reflection;
using ChooChoo.GoodsStations;
using HarmonyLib;
using JetBrains.Annotations;
using Timberborn.BaseComponentSystem;

namespace ChooChoo.GoodsStationUI
{
    [HarmonyPatch]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
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