using Bindito.Core;

namespace MorePaths.Settings
{
    [Context("MainMenu")]
    [Context("Game")]
    [Context("MapEditor")]
    public class SettingsConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<MorePathsSettings>().AsSingleton();
        }
    }
}