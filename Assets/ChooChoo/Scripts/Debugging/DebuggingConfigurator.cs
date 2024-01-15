using Bindito.Core;
using TimberApi.SceneSystem;
using TobbyTools.UsedImplicitlySystem;

namespace ChooChoo.Debugging
{
    [Configurator(SceneEntrypoint.InGame)]
    public class DebuggingConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<DebuggingMarkers>().AsSingleton();
            containerDefinition.Bind<PathFollowerDebugger>().AsSingleton();
        }
    }
}