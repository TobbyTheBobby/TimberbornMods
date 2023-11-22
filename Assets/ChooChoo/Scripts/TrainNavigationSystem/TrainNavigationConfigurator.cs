using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;

namespace ChooChoo
{
    [Configurator(SceneEntrypoint.InGame)]
    public class TrainNavigationConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<RandomTrainDestinationPicker>().AsSingleton();
            containerDefinition.Bind<TrainDestinationObjectSerializer>().AsSingleton();
            containerDefinition.Bind<TrainNavigationService>().AsSingleton();
            containerDefinition.Bind<TrainPositionDestinationFactory>().AsSingleton();
        }
    }
}