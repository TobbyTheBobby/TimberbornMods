using System.Reflection;
using HarmonyLib;
using TobbyTools.InaccessibilityUtilitySystem;
using TobbyTools.UsedImplicitlySystem;
using UnityEngine;

namespace ChooChoo.TrackSystem
{
    [UsedImplicitlyHarmonyPatch]
    public class BoundsNavRangeServicePatch
    {
        public static Material Material;

        private static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("BoundsNavRangeDrawingService"), "Load");
        }

        private static void Prefix(object ____boundsNavRangeDrawer)
        {
            Material = (Material)InaccessibilityUtilities.GetInaccessibleField(____boundsNavRangeDrawer, "_material");
        }
    }
}