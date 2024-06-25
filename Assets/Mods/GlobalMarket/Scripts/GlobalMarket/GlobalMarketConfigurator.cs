using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;

namespace GlobalMarket
{
  [Configurator(SceneEntrypoint.InGame)]
  public class GlobalMarketConfigurator : IConfigurator
  {
    public void Configure(IContainerDefinition containerDefinition)
    {
      containerDefinition.Bind<GlobalMarket>().AsSingleton();
    }
  }
}
