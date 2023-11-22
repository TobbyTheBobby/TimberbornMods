using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;

namespace ChooChoo
{
  [Configurator(SceneEntrypoint.InGame)]
  public class PowerBatchControlConfigurator : IConfigurator
  {
    public void Configure(IContainerDefinition containerDefinition)
    {
      // containerDefinition.Bind<TrainBatchControlTab>().AsSingleton();
      containerDefinition.Bind<CurrentPopulationBatchControlRowItemFactory>().AsSingleton();
      containerDefinition.Bind<TrainBatchControlRowItemFactory>().AsSingleton();
      // containerDefinition.Bind<MechanicalBatchControlRowFactory>().AsSingleton();
      // containerDefinition.MultiBind<BatchControlModule>().ToProvider<BatchControlModuleProvider>().AsSingleton();
    }

    // private class BatchControlModuleProvider : IProvider<BatchControlModule>
    // {
    //   private readonly TrainBatchControlTab _trainBatchControlTab;
    //
    //   public BatchControlModuleProvider(
    //     TrainBatchControlTab trainBatchControlTab)
    //   {
    //     _trainBatchControlTab = trainBatchControlTab;
    //   }
    //
    //   public BatchControlModule Get()
    //   {
    //     BatchControlModule.Builder builder = new BatchControlModule.Builder();
    //     builder.AddTab(_trainBatchControlTab, 9);
    //     return builder.Build();
    //   }
    // }
  }
}
