using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Timberborn.BuildingsNavigation;
using Timberborn.ModManagerScene;
using Timberborn.PrefabSystem;
using UnityEngine;

namespace Ladder
{
    public class Plugin : IModStarter
    {
        public const string PluginGuid = "tobbert.ladder";

        public void StartMod()
        {
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
                var mess = message as string;
                if (mess != null && mess.Contains("path marker mesh at"))
                {
                    return false;
                }
            }

            return __runOriginal;
        }
    }

    [HarmonyPatch(typeof(ConstructionSiteAccessible), nameof(ConstructionSiteAccessible.MinZ), MethodType.Getter)]
    public class ConstructionSiteAccessiblePatch
    {
        public static bool Prefix(ConstructionSiteAccessible __instance, ref int __result)
        {
            if (__instance._blockObject.TryGetComponentFast(out Prefab prefab) && prefab.PrefabName == "Ladder")
            {
                __result = __instance._blockObject.CoordinatesAtBaseZ.z - __instance.MaxZ;
                return false;
            }
            
            return true;
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
            return LadderService.Instance.ChangeVertical(ref pathCorners, startIndex, endIndex);
        }
    }
}