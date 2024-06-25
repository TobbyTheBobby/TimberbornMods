using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;

namespace MorePlatforms
{
    [Configurator(SceneEntrypoint.InGame)]
    internal class PluginConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<FakeParentedNeighborCalculator>().AsSingleton();
        }
    }
}