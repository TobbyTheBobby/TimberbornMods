using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.BatchControl;

namespace ChooChoo
{
  [Configurator(SceneEntrypoint.InGame)]
  public class GoodsStationBatchControlConfigurator : IConfigurator
  {
    public void Configure(IContainerDefinition containerDefinition)
    {
      containerDefinition.Bind<GoodsStationRowItemFactory>().AsSingleton();
      containerDefinition.Bind<DistributionBatchControlRowGroupFactory>().AsSingleton();
      containerDefinition.Bind<DistributionBatchControlTab>().AsSingleton();
      containerDefinition.Bind<DistributionSettingGroupFactory>().AsSingleton();
      containerDefinition.Bind<DistributionSettingsRowItemFactory>().AsSingleton();
      containerDefinition.Bind<DistrictDistributionControlRowItemFactory>().AsSingleton();
      containerDefinition.Bind<GoodDistributionSettingItemFactory>().AsSingleton();
      containerDefinition.Bind<ImportGoodIconFactory>().AsSingleton();
      containerDefinition.Bind<ImportToggleFactory>().AsSingleton();
      containerDefinition.MultiBind<BatchControlModule>().ToProvider<BatchControlModuleProvider>().AsSingleton();
    }

    private class BatchControlModuleProvider : IProvider<BatchControlModule>
    {
      private readonly DistributionBatchControlTab _distributionBatchControlTab;

      public BatchControlModuleProvider(
        DistributionBatchControlTab distributionBatchControlTab)
      {
        _distributionBatchControlTab = distributionBatchControlTab;
      }

      public BatchControlModule Get()
      {
        BatchControlModule.Builder builder = new BatchControlModule.Builder();
        builder.AddTab(_distributionBatchControlTab, DistributionBatchControlTab.TabIndex);
        return builder.Build();
      }
    }
  }
}
