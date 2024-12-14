using Bindito.Core;

namespace ChooChoo.UIPresets
{
    [Context("Game")]
    public class UIPresetsConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<PanelFragment>().AsTransient();
        }
    }
}