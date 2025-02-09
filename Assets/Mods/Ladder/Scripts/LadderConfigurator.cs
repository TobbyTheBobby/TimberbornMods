using Bindito.Core;

namespace Ladder
{
    [Context("Game")]
    public class LadderConfigurator : Configurator
    {
        protected override void Configure()
        {
            Bind<LadderService>().AsSingleton();
        }
    }
}