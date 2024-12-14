using Bindito.Core;

namespace Unstuckify.SettingsSystem
{
    [Context("MainMenu")]
    [Context("Game")]
    [Context("MapEditor")]
    public class UnstuckifySettingConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<UnstuckifySettingsOwner>().AsSingleton();
        }
    }
}