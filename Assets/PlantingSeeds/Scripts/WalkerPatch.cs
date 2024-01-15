using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Timberborn.BehaviorSystem;
using Timberborn.Carrying;
using Timberborn.InventorySystem;
using Timberborn.Navigation;

namespace PlantingSeeds
{
    [HarmonyPatch]
    public class WalkerPatch
    {
        public static IEnumerable<MethodInfo> TargetMethods() => new[]
        {
            AccessTools.Method(AccessTools.TypeByName("WalkToAccessibleExecutor"), "Launch", new []
            {
                typeof (Accessible),
                typeof(bool)
            }),
        };
    
        private static void Prefix(Accessible accessible, ref bool ignoreAccessibleValidity)
        {
            if (accessible.TryGetComponentFast(out SeedReceiver _))
            {
                ignoreAccessibleValidity = true;
            }
        }
    }
    
    
    [HarmonyPatch]
    public class LoggerPatch
    {
        public static IEnumerable<MethodInfo> TargetMethods() => new[]
        {
            AccessTools.Method(AccessTools.TypeByName("CarryRootBehavior"), "Decide", new []
            {
                typeof (BehaviorAgent),
            }),
        };
    
        private static void Prefix(GoodCarrier ____goodCarrier, GoodReserver ____goodReserver, Decision __result)
        {
            Plugin.Log.LogInfo(____goodCarrier.IsCarrying + "");
            Plugin.Log.LogInfo(____goodReserver.HasReservedStock + "");
            Plugin.Log.LogInfo(____goodReserver.HasReservedCapacity + "");
            Plugin.Log.LogInfo(__result.Executor + "");
        }
        
        private static void Postfix(GoodCarrier ____goodCarrier, GoodReserver ____goodReserver, Decision __result)
        {
            Plugin.Log.LogInfo(____goodCarrier.IsCarrying + "");
            Plugin.Log.LogInfo(____goodReserver.HasReservedStock + "");
            Plugin.Log.LogInfo(____goodReserver.HasReservedCapacity + "");
            Plugin.Log.LogInfo(__result.Executor + "");
        }
    }
    
    [HarmonyPatch]
    public class LoggerPatch2
    {
        public static IEnumerable<MethodInfo> TargetMethods() => new[]
        {
            AccessTools.Method(AccessTools.TypeByName("Constructible"), "Finish"),
        };

        private static void Postfix()
        {
            Plugin.Log.LogError("Finish");
        }
    }
}