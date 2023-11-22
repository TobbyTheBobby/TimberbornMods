using Timberborn.BatchControl;
using Timberborn.GameDistrictsUI;
using UnityEngine.UIElements;

namespace ChooChoo
{
  internal class GoodsStationRowItem : IBatchControlRowItem, IUpdateableBatchControlRowItem
  {
    private readonly GoodsStation _goodsStation;
    private readonly Label _districtNameLabel;

    public VisualElement Root { get; }

    public GoodsStationRowItem(VisualElement root, GoodsStation goodsStation, Label districtNameLabel)
    {
      Root = root;
      _goodsStation = goodsStation;
      _districtNameLabel = districtNameLabel;
    }

    public void UpdateRowItem() => _districtNameLabel.text = _goodsStation.GetComponentFast<DistrictBuildingEntityBadge>().GetEntityName();
  }
}
