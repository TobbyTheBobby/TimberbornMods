using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;

namespace Unstuckify
{
    [Configurator(SceneEntrypoint.MainMenu | SceneEntrypoint.InGame)]
    public class UnstuckifySettingConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<UnstuckifySetting>().AsSingleton();
            containerDefinition.Bind<UnstuckifySettingsUI>().AsSingleton();
        }
    }
}