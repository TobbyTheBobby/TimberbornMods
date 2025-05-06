using Bindito.Core;

namespace Unstuckify.UIPresets
{
    [Context("Game")]
    public class UIPresetsConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<PanelFragment>().AsTransient();
            containerDefinition.Bind<UnstuckifyPanel>().AsTransient();
            containerDefinition.Bind<UnstuckifyButton>().AsTransient();
        }
    }
}