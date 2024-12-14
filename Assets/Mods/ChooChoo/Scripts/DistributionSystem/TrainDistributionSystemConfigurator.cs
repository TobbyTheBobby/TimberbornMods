using Bindito.Core;

namespace ChooChoo.DistributionSystem
{
  [Context("Game")]
  internal class TrainDistributionSystemConfigurator : IConfigurator
  {
    public void Configure(IContainerDefinition containerDefinition)
    {
      containerDefinition.Bind<ChooChooCarryAmountCalculator>().AsSingleton();
    }
  }
}
