using Bindito.Core;
using System.Collections.Generic;
using Timberborn.BaseComponentSystem;
using Timberborn.BuildingsBlocking;
using Timberborn.Hauling;
using Timberborn.InventorySystem;

namespace ChooChoo
{
  internal class ObtainGoodHaulBehaviorProvider : BaseComponent, IHaulBehaviorProvider
  {
    private InventoryFillCalculator _inventoryFillCalculator;
    private GoodObtainer _goodObtainer;
    private BlockableBuilding _blockableBuilding;
    private Inventory _inventory;
    private ObtainGoodWorkplaceBehavior _obtainGoodWorkplaceBehavior;

    [Inject]
    public void InjectDependencies(InventoryFillCalculator inventoryFillCalculator) => _inventoryFillCalculator = inventoryFillCalculator;

    public void Awake()
    {
      _goodObtainer = GetComponentFast<GoodObtainer>();
      _blockableBuilding = GetComponentFast<BlockableBuilding>();
      _inventory = GetComponentFast<GoodsStation>().SendingInventory;
      _obtainGoodWorkplaceBehavior = GetComponentFast<ObtainGoodWorkplaceBehavior>();
    }

    public void GetWeightedBehaviors(IList<WeightedBehavior> weightedBehaviors)
    {
      if (!_goodObtainer.GoodObtainingEnabled || !_blockableBuilding.IsUnblocked)
        return;
      float weight = 1f - _inventoryFillCalculator.GetInputFillPercentage(_inventory);
      weightedBehaviors.Add(new WeightedBehavior(weight, _obtainGoodWorkplaceBehavior));
    }
  }
}
