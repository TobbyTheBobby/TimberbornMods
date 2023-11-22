using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using TimberApi.ConsoleSystem;
using TimberApi.DependencyContainerSystem;
using TimberApi.ModSystem;
using Timberborn.BlockSystem;
using Timberborn.Buildings;
using Timberborn.EntitySystem;
using Timberborn.Navigation;
using UnityEngine;

namespace Ladder
{
    public class Plugin : IModEntrypoint
    {
        public const string PluginGuid = "tobbert.ladder";
        
        public static IConsoleWriter Log;
        
        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            Log = consoleWriter;
            
            new Harmony(PluginGuid).PatchAll();
        }
    }
    
    [HarmonyPatch(typeof(Debug), "LogWarning", typeof(object))]
    public class LogWarningPatch
    {
        public static bool Prefix(object message, bool __runOriginal)
        {
            if (__runOriginal)
            {
                string mess = message as string;
                if (mess != null && mess.Contains("path marker mesh at"))
                {
                    return false;
                }
            }
            return __runOriginal;
        }
    }

    [HarmonyPatch]
    public class Patch3
    {
        public static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("PathReconstructor"), "TiltVerticalEdge", new[] { typeof(List<Vector3>), typeof(int), typeof(int)});
        }
        
        public static bool Prefix(ref List<Vector3> pathCorners, int startIndex, int endIndex)
        { 
            return DependencyContainer.GetInstance<LadderService>().ChangeVertical(ref pathCorners, startIndex, endIndex);
        }
    }
    
    // [HarmonyPatch(typeof(Accessible))]
    // public class Patch4
    // {
    //     public static MethodInfo TargetMethod()
    //     {
    //         return AccessTools.Method(typeof(Accessible), "FindRoadToTerrainPath", new Type[] { typeof(Accessible), typeof(Vector3).MakeByRefType(), typeof(float).MakeByRefType()});
    //     }
    //     
    //     public static void Postfix(
    //         Accessible end,
    //         Vector3 endOfRoad,
    //         float totalDistance,
    //         ref bool __result)
    //     {
    //         if (!end.Accesses.Any()) return;
    //         
    //         bool flag1 = end.Accesses.Last().x == -1;
    //         bool flag2 = end.Accesses.Last().y == -1;
    //         bool flag3 = end.Accesses.Last().z == -1;
    //         if (flag1 && flag2 && flag3)
    //         {
    //             __result = true;
    //             endOfRoad = end.Accesses.ToList()[-2];
    //             Plugin.Log.LogWarning(end.Accesses.ToList()[-1].ToString());
    //             Plugin.Log.LogWarning(end.Accesses.ToList()[-2].ToString());
    //         }
    //
    //         Plugin.Log.LogInfo(__result.ToString());
    //         Plugin.Log.LogWarning(endOfRoad.ToString());
    //         Plugin.Log.LogWarning(totalDistance.ToString());
    //     }
    // }
    //
    // [HarmonyPatch(typeof(Accessible))]
    // public class Patch5
    // {
    //     public static MethodInfo TargetMethod()
    //     {
    //         return AccessTools.Method(typeof(Accessible), "Enable", new Type[] { typeof(IEnumerable<Vector3>), typeof(Vector3)});
    //     }
    //     
    //     public static void Postfix(Accessible __instance, ref IEnumerable<Vector3> accesses)
    //     {
    //
    //         // while ((obj = TimberAPI.DependencyContainer.GetInstance<BlockService>()
    //         //        .GetObjectsWithComponentAt<Prefab>(__instance.GetComponent<BlockObject>().Coordinates)
    //         //        .Where(item => item.PrefabName.Contains("Ladder"))) ==  )
    //         // {
    //         //     var objects = .PrefabName.Contains("Ladder");
    //         // }
    //         
    //         var list = accesses.ToList();
    //
    //         var coords = __instance.GetComponent<BlockObject>().Coordinates;
    //         var checkingCoords = coords;
    //
    //         checkingCoords.y -= 1;
    //
    //         var objects = DependencyContainer.GetInstance<BlockService>().GetObjectsWithComponentAt<Prefab>(checkingCoords);
    //         foreach (var prefab in objects)
    //         {
    //             if (prefab.PrefabName.Contains("Ladder"))
    //             {
    //                 var buildingModel = prefab.GetComponent<BuildingModel>();
    //                 var type = typeof(BuildingModel).GetField("_unfinishedModel", BindingFlags.NonPublic | BindingFlags.Instance);
    //                 var value1 = type.GetValue(buildingModel) is bool ? (bool)type.GetValue(buildingModel) : false;
    //                 if (value1) continue;
    //                 list.Add(new Vector3(coords.x + 0.5f + 1, coords.z, coords.y + 0.5f));
    //                 list.Add(new Vector3(-1, -1, -1));
    //                 break;
    //             }
    //         }
    //
    //         accesses = list;
    //         
    //         Plugin.Log.LogWarning(accesses.FirstOrDefault().ToString());
    //         foreach (var VARIABLE in accesses)
    //         {
    //             Plugin.Log.LogInfo(VARIABLE.ToString());
    //         }
    //         Plugin.Log.LogInfo(__instance.GetComponent<BlockObject>().Coordinates.ToString());
    //     }
    // }
}