using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Timberborn.AssetSystem;
using TobbyTools.UsedImplicitlySystem;
using UnityEngine;

namespace MorePaths.Patches
{
    [UsedImplicitlyHarmonyPatch]
    public class PreventInstantiatePatch
    {
        public static bool RunInstantiate = true;
        
        private static IEnumerable<MethodInfo> TargetMethods()
        {
            GameObject originalPathGameObject = new ResourceAssetLoader().Load<GameObject>("Buildings/Paths/Path/Path.IronTeeth");
            var list = originalPathGameObject.GetComponents<object>();

            var methodInfoList = new List<MethodInfo>();

            var componentsWithoutAwakeFunction = new []
            {
                "Prefab",
                "BuildingConstructionRegistrar",
                "PlaceableBlockObject",
                "LabeledPrefab",
                "BuildingWithPathRange"
            };
            
            // This is useless for MorePaths, but will be used for the dynamic part that is gonna be included in TobbyTools
            var componentsWithoutStartFunction = new []
            {
                "BuildingModel",
            };

            foreach (var obj in list)
            {
                var type = obj.GetType();
                var name = type.Name;
                if (!componentsWithoutAwakeFunction.Contains(name))
                {
                    methodInfoList.Add(AccessTools.Method(type, "Awake"));
                }
                
                if (componentsWithoutStartFunction.Contains(name))
                {
                    methodInfoList.Add(AccessTools.Method(type, "Start"));
                }
            }

            return methodInfoList;
        }

        private static bool Prefix()
        {
            return RunInstantiate;
        }
    }
}