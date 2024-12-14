using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using MorePaths.CustomPaths;
using Timberborn.TemplateSystem;
using UnityEngine;

namespace MorePaths.Core
{
    [HarmonyPatch]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class TemplateInstantiatorPatch
    {
        public static IEnumerable<MethodInfo> TargetMethods()
        {
            return new[] { AccessTools.Method(typeof(TemplateInstantiator), "Instantiate", new[] { typeof(GameObject), typeof(Transform) }) };
        }

        public static void Postfix(TemplateInstantiator __instance, GameObject template, Transform parent, GameObject __result)
        {
            if (__result.TryGetComponent(out CustomPath _))
            {
                __result.SetActive(true);
                // __result.SetActive(false);
            }

            // __result.SetActive(true);
        }
    }
}