using Bindito.Core;
using TimberApi.SceneSystem;
using TobbyTools.UsedImplicitlySystem;

namespace ChooChoo.ModelSystem
{
    [Configurator(SceneEntrypoint.InGame)]
    internal class ModelSystemInGameConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<TrainModelSpecificationRepository>().AsSingleton();
        }
    }

    [Configurator(SceneEntrypoint.InGame | SceneEntrypoint.MainMenu)]
    public class ModelSystemBothConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<TrainModelSpecificationDeserializer>().AsSingleton();
            containerDefinition.Bind<WagonModelSpecificationDeserializer>().AsSingleton();
        }
    }
}