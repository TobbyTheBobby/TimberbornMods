using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using TimberApi.ConsoleSystem;
using TimberApi.DependencyContainerSystem;
using TimberApi.ModSystem;
using Timberborn.AreaSelectionSystem;
using Timberborn.AssetSystem;
using Timberborn.BlockObjectTools;
using Timberborn.BlockSystem;
using Timberborn.Buildings;
using Timberborn.Coordinates;
using Timberborn.PathSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace MorePaths
{
    public class Plugin : IModEntrypoint
    {
        public const string PluginGuid = "tobbert.morepaths";
        public static string path;
        
        public static IConsoleWriter Log;
        
        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            path = mod.DirectoryPath;
            
            Log = consoleWriter;

            new Harmony(PluginGuid).PatchAll();
        }
    }
    
    [HarmonyPatch]
    [HarmonyPriority(350)]
    public class BlockObjectToolPatch
    {
        public static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("FactionObjectCollection"), "GetObjects");
        }
        
        static void Postfix(ref IEnumerable<Object> __result)
        {
            DependencyContainer.GetInstance<MorePathsCore>().AddFakePathsToObjectsPatch(ref __result);
        }
    }
    
    
    [HarmonyPatch]
    public class PreventInstantiatePatch
    {
        public static bool RunInstantiate = true;
        
        public static IEnumerable<MethodInfo> TargetMethods()
        {
            GameObject originalPathGameObject = new ResourceAssetLoader().Load<GameObject>("Buildings/Paths/Path/Path.IronTeeth");
            var list = originalPathGameObject.GetComponents<object>();

            var methodInfoList = new List<MethodInfo>();

            List<string> componentsWithoutAwakeFunction = new List<string>
            {
                "Prefab",
                "BuildingConstructionRegistrar",
                "PlaceableBlockObject",
                "LabeledPrefab",
                "BuildingWithPathRange"
            };

            foreach (var obj in list)
            {
                var name = obj.GetType().Name;
                if (!componentsWithoutAwakeFunction.Contains(name))
                {
                    methodInfoList.Add(AccessTools.Method(AccessTools.TypeByName(name), "Awake"));
                }
            }
            
            // methodInfoList.Add(AccessTools.Method(AccessTools.TypeByName("BuildingModel"), "Start"));

            return methodInfoList;
        }
        static bool Prefix()
        {
            return RunInstantiate;
        }
    }

    [HarmonyPatch]
    public class CreateFakePathsPatch
    {
        public static MethodInfo TargetMethod()
        { 
            return AccessTools.Method(AccessTools.TypeByName("BuildingModel"), "Start");
        }
        static bool Prefix(BuildingModel __instance, BlockObjectVariantService ____blockObjectVariantService)
        {
            if (____blockObjectVariantService == null)
            {
                return false;
            }

            return true;
        }
    }
    
    // [HarmonyPatch]
    // public class UpdateBuildingsModelsAroundPatch
    // {
    //     private static MorePathsCore _morePathsCore;
    //
    //     private static MorePathsCore MorePathsCore
    //     {
    //         get { return _morePathsCore ??= DependencyContainer.GetInstance<MorePathsCore>(); }
    //     }
    //
    //     public static MethodInfo TargetMethod()
    //     { 
    //         return AccessTools.Method(AccessTools.TypeByName("BuildingModelUpdater"), "UpdateBuildingsModelsAround");
    //     }
    //     static bool Prefix(BuildingModelUpdater __instance, BlockObject blockObject)
    //     {
    //         Vector3Int coordinates = blockObject.Coordinates;
    //         Vector3Int vector = blockObject.Blocks.Size;
    //         (Vector3Int min, Vector3Int max) = Vectors.MinMax(coordinates, coordinates + blockObject.Orientation.Transform(vector));
    //         for (int z = min.z - 1; z <= max.z + 1; ++z)
    //         {
    //             for (int x = min.x - 1; x <= max.x + 1; ++x)
    //             {
    //                 MorePathsCore.InvokeInaccesableMethod(__instance, "UpdateBuildingModelsAt", new object[]{ new Vector3Int(x, min.y - 1, z) });
    //                 MorePathsCore.InvokeInaccesableMethod(__instance, "UpdateBuildingModelsAt", new object[]{ new Vector3Int(x, max.y + 1, z) });
    //             }
    //             for (int y = min.y; y <= max.y; ++y)
    //             {                    
    //                 MorePathsCore.InvokeInaccesableMethod(__instance, "UpdateBuildingModelsAt", new object[]{ new Vector3Int(min.x - 1, y, z) });
    //                 MorePathsCore.InvokeInaccesableMethod(__instance, "UpdateBuildingModelsAt", new object[]{ new Vector3Int(max.x + 1, y, z) });
    //             }
    //         }
    //
    //         return false;
    //     }
    // }

    [HarmonyPatch]
    public class DeletePathCornersPatch
    {
        public static MethodInfo TargetMethod()
        { 
            return AccessTools.Method(AccessTools.TypeByName("AreaPicker"), "PickBlockObjectArea", new[]
            {
                typeof(PlaceableBlockObject),
                typeof(Orientation),
                typeof(AreaPicker.BlockObjectAreaCallback),
                typeof(AreaPicker.BlockObjectAreaCallback)
            });
        }
        static void Prefix(PlaceableBlockObject blockObject, ref Orientation orientation)
        {
            if (blockObject.TryGetComponentFast(out DynamicPathModel _))
            {
                orientation = Orientation.Cw0;
            }
        }
    }
    
    [HarmonyPatch(typeof(BlockObjectTool), "RotateClockwise")]
    public class RotateClockwisePatch
    {
        static bool Prefix(BlockObjectTool __instance)
        {
            return !__instance.Prefab.TryGetComponentFast(out DynamicPathModel dynamicPathModel);
        }
    }
    
    [HarmonyPatch(typeof(BlockObjectTool), "RotateCounterclockwise")]
    public class RotateCounterclockwisePatch
    {
        static bool Prefix(BlockObjectTool __instance)
        {
            return !__instance.Prefab.TryGetComponentFast(out DynamicPathModel dynamicPathModel);
        }
    }
    
    [HarmonyPatch]
    public class SettingsPatch
    {
        static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("GameSavingSettingsController"), "InitializeAutoSavingOnToggle", new []
            {
                typeof(VisualElement)
            });
        }
        
        static void Postfix(ref VisualElement root)
        {
            DependencyContainer.GetInstance<MorePathsSettingsUI>().InitializeSelectorSettings(ref root);
        }
    }
}