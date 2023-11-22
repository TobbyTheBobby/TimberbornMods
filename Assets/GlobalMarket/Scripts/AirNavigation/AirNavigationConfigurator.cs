using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;

namespace GlobalMarket
{
    [Configurator(SceneEntrypoint.InGame)]
    public class AirNavigationConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<RandomAirDestinationPicker>().AsSingleton();
            containerDefinition.Bind<AirDestinationObjectSerializer>().AsSingleton();
            containerDefinition.Bind<FlightNavigationService>().AsSingleton();
            containerDefinition.Bind<AirPositionDestinationFactory>().AsSingleton();
        }
    }
}