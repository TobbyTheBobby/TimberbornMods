using System;
using Bindito.Core;
using ChooChoo.GoodsStations;
using Timberborn.BaseComponentSystem;
using Timberborn.EntityPanelSystem;
using Timberborn.EntitySystem;
using Timberborn.GameDistrictsUI;
using UnityEngine;

namespace ChooChoo.GoodsStationUI
{
    internal class GoodsStationEntityBadge : BaseComponent, IModifiableEntityBadge
    {
        private DistrictPanel _districtPanel;
        private GoodsStation _goodsStation;
        private LabeledEntity _labeledEntity;

        public event EventHandler<string> NameChanged;

        [Inject]
        public void InjectDependencies(DistrictPanel districtPanel)
        {
            _districtPanel = districtPanel;
        }

        public int EntityBadgePriority => 2;

        public void Awake()
        {
            _goodsStation = GetComponentFast<GoodsStation>();
            _labeledEntity = GetComponentFast<LabeledEntity>();
        }

        public string GetEntityName() => _goodsStation.StationName;

        public string GetEntitySubtitle() => _labeledEntity.DisplayName;

        public ClickableSubtitle GetEntityClickableSubtitle() => ClickableSubtitle.CreateEmpty();

        public Sprite GetEntityAvatar() => _labeledEntity.Image;

        public void SetEntityName(string entityName)
        {
            _goodsStation.StationName = entityName;
            _districtPanel.UpdateDistrictList();
            var nameChanged = NameChanged;
            nameChanged?.Invoke(this, entityName);
        }
    }
}
