using System.Collections.Immutable;
using TimberApi.UiBuilderSystem.CustomElements;
using Timberborn.BaseComponentSystem;
using Timberborn.BatchControl;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.GameDistricts;
using Timberborn.Localization;
using UnityEngine;
using UnityEngine.UIElements;
using LocalizableLabel = Timberborn.CoreUI.LocalizableLabel;

namespace ChooChoo
{
  public class GoodsStationFragment : IEntityPanelFragment
  {
    private static readonly string HeaderLocKey = "Tobbert.GoodsStation.Distribution.Header";

    private readonly IBatchControlBox _batchControlBox;
    private readonly BatchControlDistrict _batchControlDistrict;
    private readonly ImportGoodIconFactory _importGoodIconFactory;
    private readonly VisualElementLoader _visualElementLoader;
    private readonly ILoc _loc;
    private ImmutableArray<ImportGoodIcon> _importGoodIcons;
    private GoodsStation _goodStation;
    private GoodsStationDistributionSettings _goodsStationDistributionSettings;
    private VisualElement _root;

    public GoodsStationFragment(
      IBatchControlBox batchControlBox,
      BatchControlDistrict batchControlDistrict,
      ImportGoodIconFactory importGoodIconFactory,
      VisualElementLoader visualElementLoader,
      ILoc loc)
    {
      _batchControlBox = batchControlBox;
      _batchControlDistrict = batchControlDistrict;
      _importGoodIconFactory = importGoodIconFactory;
      _visualElementLoader = visualElementLoader;
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
      _importGoodIcons = _importGoodIconFactory.CreateImportGoods(_root.Q<VisualElement>("ImportGoodsWrapper"))
        .ToImmutableArray();
      return _root;
    }

    public void ShowFragment(BaseComponent entity)
    {
      _goodStation = entity.GetComponentFast<GoodsStation>();
      if (!(bool)(Object)_goodStation || !(bool)(Object)_goodStation.GoodsStationDistributionSettings)
        return;
      _root.ToggleDisplayStyle(true);
      SetGoodsStationDistributionSettings(_goodStation.GoodsStationDistributionSettings);
    }

    public void ClearFragment()
    {
      _goodStation = null;
      _goodsStationDistributionSettings = null;
      _root.ToggleDisplayStyle(false);
      foreach (ImportGoodIcon importGoodIcon in _importGoodIcons)
        importGoodIcon.Clear();
    }

    public void UpdateFragment()
    {
      if ((bool)(Object)_goodStation && (bool)(Object)_goodStation.GoodsStationDistributionSettings)
      {
        if (_goodsStationDistributionSettings != _goodStation.GoodsStationDistributionSettings)
          SetGoodsStationDistributionSettings(_goodStation.GoodsStationDistributionSettings);
        _root.ToggleDisplayStyle(true);
        foreach (ImportGoodIcon importGoodIcon in _importGoodIcons)
          importGoodIcon.Update();
      }
      else
        _root.ToggleDisplayStyle(false);
    }

    private void SetGoodsStationDistributionSettings(GoodsStationDistributionSettings goodsStationDistributionSettings)
    {
      _goodsStationDistributionSettings = goodsStationDistributionSettings;
      foreach (ImportGoodIcon importGoodIcon in _importGoodIcons)
        importGoodIcon.SetGoodsStationDistributionSettings(goodsStationDistributionSettings);
    }

    private void OnDistributionButtonClicked(ClickEvent evt)
    {
      _batchControlDistrict.SetDistrict(_goodStation.GetComponentFast<DistrictBuilding>().District);
      ChooChooCore.InvokePrivateMethod(_batchControlBox, "OpenTab", new object[] { DistributionBatchControlTab.TabIndex - 1 });
    }
  }
}
