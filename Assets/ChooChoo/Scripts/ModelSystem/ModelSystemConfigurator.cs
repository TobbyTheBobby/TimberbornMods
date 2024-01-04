using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;

namespace ChooChoo
{
  [Configurator(SceneEntrypoint.InGame)]
  public class ModelSystemIngameConfigurator : IConfigurator
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
