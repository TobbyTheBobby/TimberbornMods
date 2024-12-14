using Bindito.Core;
using ChooChoo.BuildingRegistrySystem;
using ChooChoo.TrackSystem;
using ChooChoo.Wagons;

namespace ChooChoo.NavigationSystem
{
    [Context("Game")]
    public class TrainNavigationSystemConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<BuildingRegistry<TrackPiece>>().AsSingleton();
            containerDefinition.Bind<BuildingRegistry<TrainDestination>>().AsSingleton();
            containerDefinition.Bind<TrainDestinationConnectedRepository>().AsSingleton();
            containerDefinition.Bind<TrackRouteWeightsCalculator>().AsSingleton();
            containerDefinition.Bind<TrainDestinationService>().AsSingleton();
            containerDefinition.Bind<WagonsObjectSerializer>().AsSingleton();

            containerDefinition.Bind<RandomTrainDestinationPicker>().AsSingleton();
            containerDefinition.Bind<TrainDestinationObjectSerializer>().AsSingleton();
            containerDefinition.Bind<TrainNavigationService>().AsSingleton();
            containerDefinition.Bind<TrainPositionDestinationFactory>().AsSingleton();
        }
    }
}