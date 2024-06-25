using Bindito.Core;
using TimberApi.SceneSystem;
using TobbyTools.UsedImplicitlySystem;

namespace TobbyTools.ImageRepository
{
    [Configurator(SceneEntrypoint.All)]
    internal class ImageRepositoryConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<ImageRepository>().AsSingleton();
            containerDefinition.Bind<ImageRepositoryService>().AsSingleton();
        }
    }
}