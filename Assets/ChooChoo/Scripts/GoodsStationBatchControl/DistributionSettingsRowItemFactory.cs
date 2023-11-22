using Timberborn.BatchControl;
using Timberborn.Common;
using Timberborn.CoreUI;
using Timberborn.EntitySystem;
using Timberborn.Goods;

namespace ChooChoo
{
  public class DistributionSettingsRowItemFactory
  {
    private readonly DistributionSettingGroupFactory _distributionSettingGroupFactory;
    private readonly GoodsGroupSpecificationService _goodsGroupSpecificationService;
    private readonly VisualElementLoader _visualElementLoader;

    public DistributionSettingsRowItemFactory(
      DistributionSettingGroupFactory distributionSettingGroupFactory,
      GoodsGroupSpecificationService goodsGroupSpecificationService,
      VisualElementLoader visualElementLoader)
    {
      _distributionSettingGroupFactory = distributionSettingGroupFactory;
      _goodsGroupSpecificationService = goodsGroupSpecificationService;
      _visualElementLoader = visualElementLoader;
    }

    public BatchControlRow Create(GoodsStationDistributionSettings goodsStationDistributionSettings)
    {
      return new BatchControlRow(_visualElementLoader.LoadVisualElement("Game/BatchControl/DistributionSettingsRowItem"), goodsStationDistributionSettings.GetComponentFast<EntityComponent>(), CreateSettingGroups(goodsStationDistributionSettings));
    }

    private ReadOnlyList<GoodGroupSpecification> GoodGroupSpecifications => _goodsGroupSpecificationService.GoodGroupSpecifications;

    private IBatchControlRowItem[] CreateSettingGroups(GoodsStationDistributionSettings goodsStationDistributionSettings)
    {
      IBatchControlRowItem[] settingGroups = new IBatchControlRowItem[GoodGroupSpecifications.Count];
      for (int index = 0; index < GoodGroupSpecifications.Count; ++index)
      {
        GoodGroupSpecification groupSpecification = GoodGroupSpecifications[index];
        settingGroups[index] = _distributionSettingGroupFactory.Create(groupSpecification, goodsStationDistributionSettings);
      }
      return settingGroups;
    }
  }
}
