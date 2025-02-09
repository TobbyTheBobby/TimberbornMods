using System.Reflection;
using System.Security.Permissions;
using HarmonyLib;
using Timberborn.BuildingsNavigation;
using Timberborn.ModManagerScene;
using Timberborn.PathSystem;
using UnityEngine;

#pragma warning disable CS0618
[assembly: SecurityPermission(action: SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

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
            if (__instance._blockObject.TryGetComponentFast(out Ladder _))
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
            return AccessTools.Method(typeof(StairsPathTransformer), "IsVerticalNeighbor", new[] { typeof(Vector3), typeof(Vector3)});
        }
        
        public static void Postfix(Vector3 nodePosition, Vector3 neighborNodePosition, ref bool __result)
        {
            if (LadderService.Instance.IsLadder(nodePosition, neighborNodePosition))
            {
                __result = false;
            }
            else
            {
                __result = true;
            }
        }
    }
}