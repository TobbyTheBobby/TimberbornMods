using System.Collections.Immutable;
using ChooChoo.GoodsStations;
using ChooChoo.GoodsStationBatchControl;
using Timberborn.BaseComponentSystem;
using Timberborn.BatchControl;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.GameDistricts;
using Timberborn.Localization;
using TobbyTools.InaccessibilityUtilitySystem;
using UnityEngine.UIElements;
using LocalizableLabel = Timberborn.CoreUI.LocalizableLabel;

namespace ChooChoo.GoodsStationUI
{
    public class GoodsStationFragment : IEntityPanelFragment
    {
        private static readonly string HeaderLocKey = "Tobbert.GoodsStation.Distribution.Header";

        private readonly ImportGoodIconFactory _importGoodIconFactory;
        private readonly BatchControlDistrict _batchControlDistrict;
        private readonly VisualElementLoader _visualElementLoader;
        private readonly IBatchControlBox _batchControlBox;
        private readonly ILoc _loc;

        private GoodsStationDistributionSettings _goodsStationDistributionSettings;
        private ImmutableArray<ImportGoodIcon> _importGoodIcons;
        private GoodsStation _goodStation;
        private VisualElement _root;

        public GoodsStationFragment(
            ImportGoodIconFactory importGoodIconFactory,
            BatchControlDistrict batchControlDistrict,
            VisualElementLoader visualElementLoader,
            IBatchControlBox batchControlBox,
            ILoc loc)
        {
            _importGoodIconFactory = importGoodIconFactory;
            _batchControlDistrict = batchControlDistrict;
            _visualElementLoader = visualElementLoader;
            _batchControlBox = batchControlBox;
            _loc = loc;
        }

        public VisualElement InitializeFragment()
        {
            _root = _visualElementLoader.LoadVisualElement("Game/EntityPanel/DistrictCrossingFragment");
            var header = _root.Q<LocalizableLabel>("Header");
            header.text = _loc.T(HeaderLocKey);
            _root.ToggleDisplayStyle(false);
            _root.Q<Button>("DistributionButton")
                .RegisterCallback(new EventCallback<ClickEvent>(OnDistributionButtonClicked));
            _importGoodIcons = _importGoodIconFactory.CreateImportGoods(_root.Q<VisualElement>("ImportGoodsWrapper")).ToImmutableArray();
            return _root;
        }

        public void ShowFragment(BaseComponent entity)
        {
            _goodStation = entity.GetComponentFast<GoodsStation>();
            if (!_goodStation || !_goodStation.GetComponentFast<GoodsStationDistributionSettings>())
                return;
            _root.ToggleDisplayStyle(true);
            SetGoodsStationDistributionSettings(_goodStation.GetComponentFast<GoodsStationDistributionSettings>());
        }

        public void ClearFragment()
        {
            _goodStation = null;
            _goodsStationDistributionSettings = null;
            _root.ToggleDisplayStyle(false);
            foreach (var importGoodIcon in _importGoodIcons)
                importGoodIcon.Clear();
        }

        public void UpdateFragment()
        {
            if (_goodStation && _goodStation.GetComponentFast<GoodsStationDistributionSettings>())
            {
                if (_goodsStationDistributionSettings != _goodStation.GetComponentFast<GoodsStationDistributionSettings>())
                    SetGoodsStationDistributionSettings(_goodStation.GetComponentFast<GoodsStationDistributionSettings>());
                _root.ToggleDisplayStyle(true);
                foreach (var importGoodIcon in _importGoodIcons)
                    importGoodIcon.Update();
            }
            else
            {
                _root.ToggleDisplayStyle(false);
            }
        }

        private void SetGoodsStationDistributionSettings(
            GoodsStationDistributionSettings goodsStationDistributionSettings)
        {
            _goodsStationDistributionSettings = goodsStationDistributionSettings;
            foreach (var importGoodIcon in _importGoodIcons)
                importGoodIcon.SetGoodsStationDistributionSettings(goodsStationDistributionSettings);
        }

        private void OnDistributionButtonClicked(ClickEvent clickEvent)
        {
            _batchControlDistrict.SetDistrict(_goodStation.GetComponentFast<DistrictBuilding>().District);
            InaccessibilityUtilities.InvokeInaccessibleMethod(_batchControlBox, "OpenTab",
                new object[] { DistributionBatchControlTab.TabIndex - 1 });
        }
    }
}