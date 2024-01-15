using System.Reflection;
using HarmonyLib;
using Timberborn.Buildings;
using Timberborn.ConstructibleSystem;
using TobbyTools.UsedImplicitlySystem;

namespace MorePaths.Patches
{
    [UsedImplicitlyHarmonyPatch]
    public class BuildingModelStartPatch
    {
        private static MethodInfo TargetMethod()
        { 
            return AccessTools.Method(AccessTools.TypeByName("BuildingModel"), "Start");
        }

        private static bool Prefix(BuildingModel __instance, Constructible ____constructible)
        {
            if (____constructible == null)
            {
                return false;
            }
    
            return true;
        }
    }
}