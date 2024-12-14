using ChooChoo.GoodsStations;
using ChooChoo.GoodsStationUI;
using Timberborn.BatchControl;
using Timberborn.EntityPanelSystem;
using Timberborn.GameDistrictsUI;
using UnityEngine;
using UnityEngine.UIElements;

namespace ChooChoo.GoodsStationBatchControl
{
    internal class GoodsStationRowItem : IBatchControlRowItem, IUpdateableBatchControlRowItem
    {
        private readonly GoodsStation _goodsStation;
        private readonly Label _stationNameLabel;
        private readonly EntityBadgeService _entityBadgeService;

        public VisualElement Root { get; }

        public GoodsStationRowItem(VisualElement root, GoodsStation goodsStation, Label stationNameLabel, EntityBadgeService entityBadgeService)
        {
            Root = root;
            _goodsStation = goodsStation;
            _stationNameLabel = stationNameLabel;
            _entityBadgeService = entityBadgeService;
        }

        public void UpdateRowItem()
        {
            _stationNameLabel.text = _entityBadgeService.GetEntityName(_goodsStation);
        }
    }
}