using System.Collections.Generic;
using Timberborn.BatchControl;
using UnityEngine.UIElements;

namespace ChooChoo
{
  public class DistributionSettingGroup : 
    IBatchControlRowItem,
    IUpdateableBatchControlRowItem,
    IClearableBatchControlRowItem
  {
    private readonly List<GoodDistributionSettingItem> _goodDistributionSettingItems;

    public VisualElement Root { get; }

    public DistributionSettingGroup(
      VisualElement root,
      List<GoodDistributionSettingItem> goodDistributionSettingItems)
    {
      Root = root;
      _goodDistributionSettingItems = goodDistributionSettingItems;
    }

    public void UpdateRowItem()
    {
      foreach (var goodDistributionSettingItem in _goodDistributionSettingItems)
        goodDistributionSettingItem.Update();
    }

    public void ClearRowItem()
    {
      foreach (var goodDistributionSettingItem   in _goodDistributionSettingItems)
        goodDistributionSettingItem.Clear();
    }
  }
}
