using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Timberborn.BlockSystem;

namespace MorePlatforms
{
    [HarmonyPatch]
    public class WorldBlockPatch
    {
        static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("WorldBlock"), "SetBlockObject", new []
            {
                typeof(BlockObject), typeof(BlockOccupations)
            });
        }
        
        static void Prefix(BlockObject blockObject, List<BlockObject> ____blockObjects)
        {
            if (blockObject.TryGetComponentFast(out OverhangingBuilding _) && !____blockObjects.Contains(blockObject))
            {
                ____blockObjects.Add(blockObject);
            }
        }
    }
}