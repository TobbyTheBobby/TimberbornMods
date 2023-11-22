using System.Collections.Generic;
using Timberborn.CoreUI;
using Timberborn.Goods;
using UnityEngine.UIElements;

namespace ChooChoo
{
  public class DistributionSettingGroupFactory
  {
    private readonly GoodDistributionSettingItemFactory _goodDistributionSettingItemFactory;
    private readonly VisualElementLoader _visualElementLoader;

    public DistributionSettingGroupFactory(
      GoodDistributionSettingItemFactory goodDistributionSettingItemFactory,
      VisualElementLoader visualElementLoader)
    {
      _goodDistributionSettingItemFactory = goodDistributionSettingItemFactory;
      _visualElementLoader = visualElementLoader;
    }

    public DistributionSettingGroup Create(GoodGroupSpecification groupSpecification, GoodsStationDistributionSettings goodsStationDistributionSettings)
    {
      VisualElement visualElement = _visualElementLoader.LoadVisualElement("Game/BatchControl/DistributionSettingsGroup");
      visualElement.Q<Image>("Icon").sprite = groupSpecification.Icon;
      List<GoodDistributionSettingItem> items = CreateItems(goodsStationDistributionSettings, groupSpecification.Id, visualElement.Q<VisualElement>("Items"));
      return new DistributionSettingGroup(visualElement, items);
    }

    private List<GoodDistributionSettingItem> CreateItems(GoodsStationDistributionSettings goodsStationDistributionSettings, string groupId, VisualElement parent)
    {
      List<GoodDistributionSettingItem> items = new List<GoodDistributionSettingItem>();
      GoodsStationDistributionSettings componentFast = goodsStationDistributionSettings.GetComponentFast<GoodsStation>().GoodsStationDistributionSettings;
      foreach (GoodsStationGoodDistributionSetting goodDistributionSetting in goodsStationDistributionSettings.GetGoodDistributionSettingsForGroup(groupId))
      {
        GoodDistributionSettingItem distributionSettingItem = _goodDistributionSettingItemFactory.Create(componentFast, goodDistributionSetting);
        items.Add(distributionSettingItem);
        parent.Add(distributionSettingItem.Root);
      }
      return items;
    }
  }
}
