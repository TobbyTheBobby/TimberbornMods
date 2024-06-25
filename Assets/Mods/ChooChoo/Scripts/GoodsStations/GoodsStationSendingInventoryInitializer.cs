using Timberborn.BaseComponentSystem;
using Timberborn.Goods;
using Timberborn.InventorySystem;
using Timberborn.TemplateSystem;

namespace ChooChoo.GoodsStations
{
    internal class GoodsStationSendingInventoryInitializer : IDedicatedDecoratorInitializer<GoodsStationSendingInventory, Inventory>
    {
        private static readonly string InventoryComponentName = nameof(GoodsStationSendingInventory);
        private readonly InventoryInitializerFactory _inventoryInitializerFactory;
        private readonly BaseInstantiator _baseInstantiator;
        private readonly IGoodService _goodService;

        public GoodsStationSendingInventoryInitializer(
            InventoryInitializerFactory inventoryInitializerFactory, 
            BaseInstantiator baseInstantiator,
            IGoodService goodService)
        {
            _inventoryInitializerFactory = inventoryInitializerFactory;
            _baseInstantiator = baseInstantiator;
            _goodService = goodService;
        }

        public void Initialize(GoodsStationSendingInventory subject, Inventory decorator)
        {
            var unlimitedCapacity = _inventoryInitializerFactory.CreateWithUnlimitedCapacity(decorator, InventoryComponentName);
            AllowEveryGoodAsGiveAble(unlimitedCapacity);
            unlimitedCapacity.HasPublicOutput();
            unlimitedCapacity.SetIgnorableCapacity();
            var limitableGoodDisallower = _baseInstantiator.AddComponent<LimitableGoodDisallower>(subject.GameObjectFast);
            unlimitedCapacity.AddGoodDisallower(limitableGoodDisallower);
            limitableGoodDisallower.SetComponentName(InventoryComponentName);
            unlimitedCapacity.Initialize();
            subject.InitializeSendingInventory(decorator, limitableGoodDisallower);
        }

        private void AllowEveryGoodAsGiveAble(InventoryInitializer inventoryInitializer)
        {
            foreach (var good1 in _goodService.Goods)
            {
                var good2 = new StorableGoodAmount(StorableGood.CreateAsGivable(good1), GoodsStation.Capacity);
                inventoryInitializer.AddAllowedGood(good2);
            }
        }
    }
}