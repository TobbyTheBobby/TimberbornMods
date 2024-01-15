using Bindito.Core;
using ChooChoo.Wagons;
using TimberApi.SceneSystem;
using TobbyTools.BuildingRegistrySystem;
using TobbyTools.UsedImplicitlySystem;

namespace ChooChoo.NavigationSystem
{
    [Configurator(SceneEntrypoint.InGame)]
    public class TrainNavigationSystemConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
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