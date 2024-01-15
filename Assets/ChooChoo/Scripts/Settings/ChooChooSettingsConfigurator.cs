using Bindito.Core;
using TimberApi.SceneSystem;
using TobbyTools.UsedImplicitlySystem;

namespace ChooChoo.Settings
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