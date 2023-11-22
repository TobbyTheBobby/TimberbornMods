using Bindito.Core;
using System.Collections.Generic;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.Localization;
using UnityEngine;

namespace ChooChoo
{
  public class GoodsStationDescriber : MonoBehaviour, IEntityDescriber
  {
    private static readonly string CapacityLocKey = "Inventory.Capacity";
    private ILoc _loc;
    private GoodsStation _stockpile;

    [Inject]
    public void InjectDependencies(ILoc loc) => _loc = loc;

    public void Awake() => _stockpile = GetComponent<GoodsStation>();

    public IEnumerable<EntityDescription> DescribeEntity()
    {
      if (!_stockpile.enabled)
        yield return EntityDescription.CreateTextSection($"{SpecialStrings.RowStarter}{_loc.T(CapacityLocKey)} {_stockpile.MaxCapacity}", 100);
    }
  }
}
