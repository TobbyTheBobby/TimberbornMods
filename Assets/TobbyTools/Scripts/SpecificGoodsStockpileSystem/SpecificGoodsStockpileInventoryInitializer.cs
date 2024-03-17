using System.Linq;
using Timberborn.BaseComponentSystem;
using Timberborn.Goods;
using Timberborn.InventoryNeedSystem;
using Timberborn.InventorySystem;
using Timberborn.TemplateSystem;

namespace TobbyTools.SpecificGoodsStockpileSystem
{
    public class SpecificGoodsStockpileInventoryInitializer : IDedicatedDecoratorInitializer<SpecificGoodsStockpile, Inventory>
    {
        private static readonly string InventoryComponentName = nameof(SpecificGoodsStockpile);
        private readonly InventoryInitializerFactory _inventoryInitializerFactory;
        private readonly InventoryNeedBehaviorAdder _inventoryNeedBehaviorAdder;

        public SpecificGoodsStockpileInventoryInitializer(
            InventoryInitializerFactory inventoryInitializerFactory,
            InventoryNeedBehaviorAdder inventoryNeedBehaviorAdder)
        {
            _inventoryInitializerFactory = inventoryInitializerFactory;
            _inventoryNeedBehaviorAdder = inventoryNeedBehaviorAdder;
        }

        public void Initialize(SpecificGoodsStockpile subject, Inventory decorator)
        {
            var inventoryInitializer = _inventoryInitializerFactory.CreateWithUnlimitedCapacity(decorator, InventoryComponentName);
            inventoryInitializer.AddAllowedGoods(subject.StorableGoods.Select(specification =>
            {
                Plugin.Log.LogError(specification.ToString());
                return new StorableGoodAmount(StorableGood.CreateGiveableAndTakeable(specification.GoodId), specification.Amount);
            }));
            inventoryInitializer.HasPublicOutput();
            inventoryInitializer.HasPublicInput();
            inventoryInitializer.Initialize();
            subject.InitializeInventory(decorator);
            _inventoryNeedBehaviorAdder.AddNeedBehavior(decorator);
        }
    }
}