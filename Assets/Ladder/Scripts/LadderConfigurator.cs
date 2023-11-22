using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;

namespace Ladder
{
    [Configurator(SceneEntrypoint.InGame)]
    public class LadderConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<LadderService>().AsSingleton();
        }
    }
}