using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using TobbyTools.UsedImplicitlySystem;
using UnityEngine;

namespace MorePaths.Patches
{
    [UsedImplicitlyHarmonyPatch]
    [HarmonyPriority(350)]
    public class FactionObjectCollectionGetObjectsPatch
    {
        private static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("FactionObjectCollection"), "GetObjects");
        }

        private static void Postfix(ref IEnumerable<Object> __result)
        {
            // Plugin.Log.LogInfo("Replacing all default paths with custom paths.");
            
            var pathGameObject = __result.First(o => o.name.Split(".")[0] == "Path");
            if (pathGameObject == null) 
                return;

            var resultList = __result.ToList();
            resultList.Remove(pathGameObject);

            __result = resultList;
        }
    }
}