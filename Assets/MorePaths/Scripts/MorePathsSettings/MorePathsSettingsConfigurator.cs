using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;

namespace MorePaths
{
    [Configurator(SceneEntrypoint.MainMenu | SceneEntrypoint.InGame)]
    public class MorePathsSettingsConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<MorePathsSettingsUI>().AsSingleton();
            containerDefinition.Bind<MorePathsSettings>().AsSingleton();
        }
    }
    
    [Configurator(SceneEntrypoint.InGame)]
    public class MorePathsSettingsInGameConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<MorePathsSettingsController>().AsSingleton();
        }
    }
}