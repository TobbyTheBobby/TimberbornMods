using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Timberborn.EntitySystem;
using Timberborn.InventorySystem;
using Timberborn.StockpilesUI;
using UnityEngine;

namespace ChooChoo
{
  internal class GoodsStationOptionsProvider : MonoBehaviour, IInitializableEntity
  {
    private Inventory _inventory;

    public ImmutableArray<string> Options { get; private set; }

    public void Awake() => _inventory = GetComponent<GoodsStation>().SendingInventory;

    public void InitializeEntity()
    {
      IEnumerable<string> collection = _inventory.AllowedGoods.Select(good => good.StorableGood.GoodId);
      List<string> stringList = new List<string>();
      stringList.AddRange(collection);
      stringList.Add(StockpileOptionsService.NothingSelectedLocKey);
      Options = stringList.ToImmutableArray();
    }
  }
}
