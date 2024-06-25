using Bindito.Core;
using MorePaths.Core;
using TimberApi.SceneSystem;
using TobbyTools.UsedImplicitlySystem;

namespace MorePaths.Specifications
{
    [Configurator(SceneEntrypoint.All)]
    public class MorePathsConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<PathSpecificationObjectDeserializer>().AsSingleton();
            containerDefinition.Bind<MorePathsCore>().AsSingleton();
        }
    }
}