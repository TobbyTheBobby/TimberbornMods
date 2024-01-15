using System.Reflection;
using Bindito.Core;
using HarmonyLib;

namespace TobbyTools
{
    [UsedImplicitlySystem.UsedImplicitlyHarmonyPatch]
    public class BuildingSpecificationConfiguratorPatch
    {
        static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("BuildingSpecificationConfigurator"), "Configure", new []
            {
                typeof(IContainerDefinition)
            });
        }
        
        static bool Prefix()
        {
            return false;
        }
    }
}