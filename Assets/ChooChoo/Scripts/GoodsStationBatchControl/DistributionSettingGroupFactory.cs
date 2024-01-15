using System.Collections.Generic;
using ChooChoo.GoodsStations;
using Timberborn.CoreUI;
using Timberborn.Goods;
using UnityEngine.UIElements;

namespace ChooChoo.GoodsStationBatchControl
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

        public DistributionSettingGroup Create(GoodGroupSpecification groupSpecification,
            GoodsStationDistributionSettings goodsStationDistributionSettings)
        {
            var visualElement = _visualElementLoader.LoadVisualElement("Game/BatchControl/DistributionSettingsGroup");
            visualElement.Q<Image>("Icon").sprite = groupSpecification.Icon;
            var goodDistributionSettings =
                CreateItems(goodsStationDistributionSettings, groupSpecification.Id, visualElement.Q<VisualElement>("Items"));
            return new DistributionSettingGroup(visualElement, goodDistributionSettings);
        }

        private List<GoodDistributionSettingItem> CreateItems(GoodsStationDistributionSettings goodsStationDistributionSettings, string groupId,
            VisualElement parent)
        {
            var items = new List<GoodDistributionSettingItem>();
            var componentFast = goodsStationDistributionSettings.GetComponentFast<GoodsStationDistributionSettings>();
            foreach (var goodDistributionSetting in goodsStationDistributionSettings.GetGoodDistributionSettingsForGroup(groupId))
            {
                var distributionSettingItem = _goodDistributionSettingItemFactory.Create(componentFast, goodDistributionSetting);
                items.Add(distributionSettingItem);
                parent.Add(distributionSettingItem.Root);
            }

            return items;
        }
    }
}