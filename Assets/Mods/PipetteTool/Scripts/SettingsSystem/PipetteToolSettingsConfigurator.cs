using Bindito.Core;
using ModSettings.CoreUI;

namespace PipetteTool.SettingsSystem
{
    [Context("MainMenu")]
    [Context("Game")]
    [Context("MapEditor")]
    public class PipetteToolSettingsConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.MultiBind<IModSettingElementFactory>().To<KeybindingModSettingElementFactory>().AsSingleton();
            containerDefinition.Bind<PipetteToolSettingsOwner>().AsSingleton();
        }
    }
}