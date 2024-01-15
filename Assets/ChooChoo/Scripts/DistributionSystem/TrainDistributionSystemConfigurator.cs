using Bindito.Core;
using TimberApi.SceneSystem;
using TobbyTools.UsedImplicitlySystem;

namespace ChooChoo.DistributionSystem
{
  [Configurator(SceneEntrypoint.InGame)]
  internal class TrainDistributionSystemConfigurator : IConfigurator
  {
    public void Configure(IContainerDefinition containerDefinition)
    {
      containerDefinition.Bind<ChooChooCarryAmountCalculator>().AsSingleton();
    }
  }
}
