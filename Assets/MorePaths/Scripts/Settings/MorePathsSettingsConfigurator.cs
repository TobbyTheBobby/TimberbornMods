using Bindito.Core;
using TimberApi.SceneSystem;
using TobbyTools.UsedImplicitlySystem;

namespace MorePaths.Settings
{
    [Configurator(SceneEntrypoint.MainMenu | SceneEntrypoint.InGame)]
    public class SettingsConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<MorePathsSettingsUI>().AsSingleton();
            containerDefinition.Bind<MorePathsSettings>().AsSingleton();
        }
    }
}