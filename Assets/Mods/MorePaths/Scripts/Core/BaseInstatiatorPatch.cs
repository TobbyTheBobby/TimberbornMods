using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Timberborn.BaseComponentSystem;
using Timberborn.PathSystem;
using UnityEngine;

namespace MorePaths.Core
{
    [HarmonyPatch]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class BaseInstatiatorPatch
    {
        public static IEnumerable<MethodInfo> TargetMethods()
        {
            return new[] { AccessTools.Method(typeof(BaseInstantiator), "InstantiateInactive", new[] { typeof(GameObject), typeof(Transform) }) };
        }

        public static void Postfix(GameObject __result)
        {
            if (!__result.GetComponent<DynamicPathModel>())
                return;
            var componentCache = __result.GetComponent<ComponentCache>();

            if (componentCache._components == null) componentCache.Initialize();
        }
    }
}