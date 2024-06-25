using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;

namespace CustomCursors
{
    [Configurator(SceneEntrypoint.All)]
    public class CustomCursorsConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<CustomCursorsService>().AsSingleton();
        }
    }
}