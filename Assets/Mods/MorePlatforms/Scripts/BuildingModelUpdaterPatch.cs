using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Timberborn.BlockSystem;
using Timberborn.Buildings;
using Timberborn.Common;
using Timberborn.Coordinates;
using UnityEngine;

namespace MorePlatforms
{
    [HarmonyPatch]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class BuildingModelUpdaterPatch
    {
        private static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("BuildingModelUpdater"), "UpdateBuildingsModelsAround", new []
            {
                typeof(BlockObject)
            });
        }

        private static bool Prefix(BuildingModelUpdater __instance, BlockObject blockObject)
        {
            if (!blockObject.TryGetComponentFast(out OverhangingBuilding _))
                return true;
            var coordinates = blockObject.Coordinates;
            var vector = blockObject.Blocks.Size - new Vector3Int(1, 1, 1);
            var (min, max) = Vectors.MinMax(coordinates, coordinates + blockObject.Orientation.Transform(vector));
            for (var z = min.z - 1; z <= max.z + 1; ++z)
            {
                for (var x = min.x - 1; x <= max.x + 1; ++x)
                {

                    __instance.UpdateBuildingModelsAt(new Vector3Int(x, min.y - 1, z));
                    __instance.UpdateBuildingModelsAt(new Vector3Int(x, max.y + 1, z));
                }
                for (var y = min.y - 1; y <= max.y + 1; ++y)
                {
                    __instance.UpdateBuildingModelsAt(new Vector3Int(min.x - 1, y, z));
                    __instance.UpdateBuildingModelsAt(new Vector3Int(max.x + 1, y, z));
                }
            }

            return false;   
        }
    }
}