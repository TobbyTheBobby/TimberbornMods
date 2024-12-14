using Bindito.Core;
using HarmonyLib;
using JetBrains.Annotations;
using MorePaths.CustomPaths;
using Timberborn.AssetSystem;
using Timberborn.Bootstrapper;

namespace MorePaths.Core
{
    [HarmonyPatch]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class BootstrapperConfiguratorPatch
    {
        [HarmonyPatch(typeof(BootstrapperConfigurator), "Configure")]
        [HarmonyPrefix]
        public static void Prefix(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<CustomPathsRepository>().AsSingleton();
            containerDefinition.MultiBind<IAssetProvider>().To<CustomPathsAssetsProvider>().AsSingleton();
        }
    }
}