using Bindito.Core;

namespace ChooChoo.MovementSystem
{
    [Context("Game")]
    public class TrainMovementSystemConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<TrackFollowerFactory>().AsSingleton();
        }
    }
}