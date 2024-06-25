using Bindito.Core;
using TimberApi.SceneSystem;
using TobbyTools.UsedImplicitlySystem;

namespace ChooChoo.MovementSystem
{
    [Configurator(SceneEntrypoint.InGame)]
    public class TrainMovementSystemConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<TrackFollowerFactory>().AsSingleton();
        }
    }
}