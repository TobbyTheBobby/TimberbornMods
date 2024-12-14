using System;
using System.Collections.Generic;
using System.Reflection;
using Bindito.Core;
using HarmonyLib;
using JetBrains.Annotations;
using TimberApi;
using Timberborn.BlockSystem;
using Timberborn.PrefabOptimization;
using Timberborn.TimbermeshMaterials;

namespace MorePaths.Core
{
    [HarmonyPatch]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class DoubleLoaderPrevention
    {
        private static List<Type> _alreadyLoaded = new();

        public static IEnumerable<MethodInfo> TargetMethods()
        {
            SceneManager.SceneChanged += OnSceneChanged;

            foreach (var type in new List<Type>
                     {
                         // typeof(PreviewBlockService),
                         // typeof(BlockService),
                         typeof(AutoAtlaser),
                         typeof(MaterialRepository)
                     })
            {
                yield return AccessTools.Method(type, "Load");
            }
        }

        public static bool Prefix(object __instance)
        {
            var type = __instance.GetType();
            if (_alreadyLoaded.Contains(type))
                return false;
            
            _alreadyLoaded.Add(__instance.GetType());
            return true;
        }

        private static void OnSceneChanged(Scene previousscene, Scene currentscene, IContainerDefinition currentcontainerdefinition)
        {
            _alreadyLoaded.Clear();
        }
    }
}