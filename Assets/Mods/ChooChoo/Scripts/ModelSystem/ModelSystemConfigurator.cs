using Bindito.Core;

namespace ChooChoo.ModelSystem
{
    [Context("Game")]
    internal class ModelSystemInGameConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<TrainModelSpecificationRepository>().AsSingleton();
        }
    }

    [Context("Game")]
    [Context("MainMenu")]
    public class ModelSystemBothConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<TrainModelSpecificationDeserializer>().AsSingleton();
            containerDefinition.Bind<WagonModelSpecificationDeserializer>().AsSingleton();
        }
    }
}