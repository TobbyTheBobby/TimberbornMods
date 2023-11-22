using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Timberborn.BehaviorSystem;
using Timberborn.Planting;

namespace PlantingSeeds
{
    public class NavigationRangeSericePatch
    {
        [HarmonyPatch]
        public class RoadNavMeshGraphPatch
        {
            public static IEnumerable<MethodInfo> TargetMethods() => new[]
            {
                AccessTools.Method(AccessTools.TypeByName("PlantBehavior"), "Decide", new []
                {
                    typeof (BehaviorAgent),
                }),
            };

            private static bool Prefix(Planter ____planter, ref Decision __result)
            {
                if (____planter.GetComponentFast<PlantSeedBehavior>().Decide(out var decision)) 
                    return true;
                __result = decision;
                return false;
            }
        }
    }
}