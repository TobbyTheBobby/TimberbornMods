using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Timberborn.BuildingsNavigation;
using UnityEngine;

namespace ChooChoo.TrackSystem
{
    [HarmonyPatch]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class BoundsNavRangeServicePatch
    {
        public static Material Material;

        private static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("BoundsNavRangeDrawingService"), "Load");
        }

        private static void Prefix(BoundsNavRangeDrawer ____boundsNavRangeDrawer)
        {
            Material = ____boundsNavRangeDrawer._material;
        }
    }
}