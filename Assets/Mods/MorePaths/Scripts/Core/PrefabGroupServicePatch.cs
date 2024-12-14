using HarmonyLib;
using JetBrains.Annotations;
using MorePaths.CustomPaths;
using TimberApi;
using Timberborn.BlockSystem;
using Timberborn.PrefabGroupSystem;
using Timberborn.PrefabOptimization;
using Timberborn.SingletonSystem;
using Timberborn.Timbermesh;
using Timberborn.TimbermeshMaterials;

namespace MorePaths.Core
{
    [HarmonyPatch]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class PrefabGroupServicePatch
    {
        [HarmonyPatch(typeof(PrefabGroupService), "Load")]
        [HarmonyPrefix]
        public static void Prefix()
        {
            if (SceneManager.CurrentScene == Scene.MapEditor)
                return;
            
            EarlyLoad<PreviewBlockService>();
            EarlyLoad<BlockService>();
            EarlyLoad<AutoAtlaser>();
            EarlyLoad<MaterialRepository, IMaterialRepository>();
            
            TimberApi.DependencyContainerSystem.DependencyContainer.GetInstance<CustomPathGenerator>().VeryEarlyLoad();
        }

        private static void EarlyLoad<T>() where T : ILoadableSingleton
        {
            TimberApi.DependencyContainerSystem.DependencyContainer.GetInstance<T>().Load();
        }

        private static void EarlyLoad<T, TT>() where T : class, ILoadableSingleton
        {
            (TimberApi.DependencyContainerSystem.DependencyContainer.GetInstance<TT>() as T)?.Load();
        }
    }
}