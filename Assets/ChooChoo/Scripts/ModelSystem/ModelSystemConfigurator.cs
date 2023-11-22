using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;

namespace ChooChoo
{
  [Configurator(SceneEntrypoint.InGame)]
  public class ModelSystemConfigurator : IConfigurator
  {
    public void Configure(IContainerDefinition containerDefinition)
    {
      containerDefinition.Bind<TrainModelSpecificationRepository>().AsSingleton();
      containerDefinition.Bind<TrainModelSpecificationDeserializer>().AsSingleton();
      containerDefinition.Bind<WagonModelSpecificationDeserializer>().AsSingleton();
    }
  }
}
