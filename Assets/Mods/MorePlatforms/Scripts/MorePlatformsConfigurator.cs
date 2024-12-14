using Bindito.Core;

namespace MorePlatforms
{
    [Context("Game")]
    internal class PluginConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<FakeParentedNeighborCalculator>().AsSingleton();
        }
    }
}