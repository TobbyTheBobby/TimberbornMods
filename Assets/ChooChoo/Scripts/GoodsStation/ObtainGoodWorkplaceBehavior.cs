using System.Collections.Generic;
using System.Linq;
using Bindito.Core;
using Timberborn.BehaviorSystem;
using Timberborn.Carrying;
using Timberborn.Goods;
using Timberborn.InventorySystem;
using Timberborn.WorkSystem;

namespace ChooChoo
{
  internal class ObtainGoodWorkplaceBehavior : WorkplaceBehavior
  {
    private IGoodService _goodService;
    
    private Inventory _inventory;
    private GoodObtainer _goodObtainer;
    private LimitableGoodDisallower _limitableGoodDisallower;

    [Inject]
    public void InjectDependencies(IGoodService goodService)
    {
      _goodService = goodService;
    }
    
    public void Awake()
    {
      _inventory = GetComponentFast<GoodsStation>().SendingInventory;
      _goodObtainer = GetComponentFast<GoodObtainer>();
      var limitableGoodDisallowers = new List<LimitableGoodDisallower>();
      GetComponentsFast(limitableGoodDisallowers);
      _limitableGoodDisallower = limitableGoodDisallowers.First();
    }

    public override Decision Decide(BehaviorAgent agent)
    {
      return CanObtain() && StartCarrying(agent) ? Decision.ReleaseNextTick() : Decision.ReleaseNow();
    }

    private bool CanObtain()
    {
      return _goodObtainer.GoodObtainingEnabled && _inventory.enabled && _limitableGoodDisallower.HasAllowedGoods;
    }

    private bool StartCarrying(BehaviorAgent agent)
    {
      return _goodService.Goods.Any(good => agent.GetComponentFast<CarrierInventoryFinder>().TryCarryFromAnyInventory(good, _inventory, CanObtainFrom));
    }

    private static bool CanObtainFrom(Inventory inventory)
    {
      var componentFast1 = inventory.GetComponentFast<Timberborn.StockpilePrioritySystem.GoodObtainer>();
      var componentFast2 = inventory.GetComponentFast<GoodObtainer>();
      var cannotObtainFrom = (componentFast1 != null && componentFast1.GoodObtainingEnabled) || 
                             (componentFast2 != null && componentFast2.GoodObtainingEnabled);
      return !cannotObtainFrom;
    }
  }
}
