using JetBrains.Annotations;
using TimberApi.SceneSystem;

namespace TobbyTools.UsedImplicitlySystem
{
    [MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
    [UsedImplicitly]
    public class ConfiguratorAttribute : TimberApi.ConfiguratorSystem.ConfiguratorAttribute
    {
        public ConfiguratorAttribute(SceneEntrypoint entryPoint) : base(entryPoint)
        {
            
        }
    }
}