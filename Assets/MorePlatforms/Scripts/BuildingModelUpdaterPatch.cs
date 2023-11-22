using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Timberborn.BlockSystem;
using Timberborn.Buildings;
using Timberborn.Common;
using Timberborn.Coordinates;
using UnityEngine;

namespace MorePlatforms
{
    [HarmonyPatch]
    public class BuildingModelUpdaterPatch
    {
        static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("BuildingModelUpdater"), "UpdateBuildingsModelsAround", new []
            {
                typeof(BlockObject)
            });
        }
        
        static bool Prefix(BuildingModelUpdater __instance, BlockObject blockObject)
        {
            if (!blockObject.TryGetComponentFast(out OverhangingBuilding _))
                return true;
            Vector3Int coordinates = blockObject.Coordinates;
            Vector3Int vector = blockObject.Blocks.Size - new Vector3Int(1, 1, 1);
            (Vector3Int min, Vector3Int max) = Vectors.MinMax(coordinates, coordinates + blockObject.Orientation.Transform(vector));
            for (int z = min.z - 1; z <= max.z + 1; ++z)
            {
                for (int x = min.x - 1; x <= max.x + 1; ++x)
                {
                    MorePlatformsCore.InvokePrivateMethod(__instance, "UpdateBuildingModelsAt", new object[] { new Vector3Int(x, min.y - 1, z) });
                    MorePlatformsCore.InvokePrivateMethod(__instance, "UpdateBuildingModelsAt", new object[] { new Vector3Int(x, max.y + 1, z) });
                }
                for (int y = min.y - 1; y <= max.y + 1; ++y)
                {
                    MorePlatformsCore.InvokePrivateMethod(__instance, "UpdateBuildingModelsAt", new object[] { new Vector3Int(min.x - 1, y, z) });
                    MorePlatformsCore.InvokePrivateMethod(__instance, "UpdateBuildingModelsAt", new object[] { new Vector3Int(max.x + 1, y, z) });
                }
            }

            return false;   
        }
    }
}