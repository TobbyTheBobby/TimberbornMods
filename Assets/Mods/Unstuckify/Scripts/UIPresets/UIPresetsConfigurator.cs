using Bindito.Core;
using TimberApi.UIPresets.Buttons;

namespace Unstuckify.UIPresets
{
    [Context("Game")]
    public class UIPresetsConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<PanelFragment>().AsTransient();
            containerDefinition.Bind<GameButton>().AsTransient();
            containerDefinition.Bind<UnstuckifyPanel>().AsTransient();
            containerDefinition.Bind<UnstuckifyButton>().AsTransient();
        }
    }
}