using Bindito.Core;

namespace ChooChoo.Settings
{
    [Context("MainMenu")]
    [Context("Game")]
    public class ChooChooSettingsConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<TrainTypeSettingDropdownProvider>().AsSingleton();
            containerDefinition.Bind<WagonTypeSettingDropdownProvider>().AsSingleton();
            containerDefinition.Bind<ChooChooSettings>().AsSingleton();
            // containerDefinition.Bind<ChooChooSettingsUI>().AsSingleton();
        }
    }
}