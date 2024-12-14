using Bindito.Core;
using HarmonyLib;
using Timberborn.Bootstrapper;

namespace CustomCursors
{
    [HarmonyPatch]
    public class BootstrapperConfiguratorPatch
    {
        [HarmonyPatch(typeof(BootstrapperConfigurator), "Configure")]
        [HarmonyPrefix]
        public static void Prefix(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<CursorPackRepository>().AsSingleton();
        }
    }
}