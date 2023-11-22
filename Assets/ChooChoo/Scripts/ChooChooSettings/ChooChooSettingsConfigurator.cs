using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;

namespace ChooChoo
{
    [Configurator(SceneEntrypoint.MainMenu | SceneEntrypoint.InGame)]
    public class ChooChooSettingsConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<TrainTypeSettingDropdownProvider>().AsSingleton();
            containerDefinition.Bind<WagonTypeSettingDropdownProvider>().AsSingleton();
            containerDefinition.Bind<ChooChooSettings>().AsSingleton();
            containerDefinition.Bind<ChooChooSettingsUI>().AsSingleton();
        }
    }
}