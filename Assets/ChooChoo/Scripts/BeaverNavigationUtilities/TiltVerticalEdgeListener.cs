using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using TimberApi.DependencyContainerSystem;
using UnityEngine;

namespace ChooChoo
{
    [HarmonyPatch, UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class TiltVerticalEdgeListener
    {
        public static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("PathReconstructor"), "TiltVerticalEdge", new[]
            {
                typeof(List<Vector3>), 
                typeof(int),
                typeof(int)
            });
        }
        
        public static bool Prefix(ref List<Vector3> pathCorners, int startIndex, int endIndex)
        {
            var runOriginal = !DependencyContainer.GetInstance<PathCorrector>().IsBetweenPassengerStations(ref pathCorners, startIndex, endIndex);
            return runOriginal;
        }
    }
}