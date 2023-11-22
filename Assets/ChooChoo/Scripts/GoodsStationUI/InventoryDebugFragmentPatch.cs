using System.Reflection;
using HarmonyLib;
using Timberborn.BaseComponentSystem;

namespace ChooChoo
{
    [HarmonyPatch]
    public class InventoryDebugFragmentPatch
    {
        static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("InventoryDebugFragment"), "ShowFragment", new []{ typeof(BaseComponent)});
        }
        
        static bool Prefix(BaseComponent entity)
        {

            if (entity.TryGetComponentFast(out GoodsStation _))
            {
                return false;
            }

            return true;
        }
    }
}