using Bindito.Core;

namespace MorePaths.PathSpecificationSystem
{
    [Context("MainMenu")]
    [Context("Game")]
    [Context("MapEditor")]
    public class MorePathsConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<PathSpecificationRepository>().AsSingleton();
            containerDefinition.Bind<PathSpecificationObjectDeserializer>().AsSingleton();
        }
    }
}