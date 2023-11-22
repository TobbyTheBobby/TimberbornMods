using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace ChooChoo
{
    
    [HarmonyPatch, UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class BoundsNavRangeServicePatch
    {
        public static Material Material;
        
        static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("BoundsNavRangeDrawingService"), "Load");
        }
        
        static void Prefix(object ____boundsNavRangeDrawer)
        {
            Material = ChooChooCore.GetInaccessibleField(____boundsNavRangeDrawer, "_material") as Material;
        }
    }
}