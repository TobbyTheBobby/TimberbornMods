using ChooChoo.Trains;
using Timberborn.AssetSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.Goods;
using Timberborn.InventorySystem;
using Timberborn.TemplateSystem;
using UnityEngine;

namespace ChooChoo.TrainYards
{
    internal class TrainYardInventoryInitializer : IDedicatedDecoratorInitializer<TrainYard, Inventory>
    {
        private static readonly string InventoryComponentName = "TrainYard";
        private readonly IGoodService _goodService;
        private readonly BaseInstantiator _baseInstantiator;
        private readonly IResourceAssetLoader _resourceAssetLoader;

        public TrainYardInventoryInitializer(IGoodService goodService, BaseInstantiator baseInstantiator, IResourceAssetLoader resourceAssetLoader)
        {
            _goodService = goodService;
            _baseInstantiator = baseInstantiator;
            _resourceAssetLoader = resourceAssetLoader;
        }

        public void Initialize(TrainYard subject, Inventory decorator)
        {
            var inventoryInitializer = new InventoryInitializer(_goodService, _baseInstantiator, decorator, CalculateTotalCapacity(subject),
                InventoryComponentName);
            inventoryInitializer.HasPublicInput();
            inventoryInitializer.HasPublicOutput();
            AllowEveryGoodAsGiveAndTakeable(inventoryInitializer,
                _resourceAssetLoader.Load<GameObject>("tobbert.choochoo/tobbert_choochoo/Train").GetComponent<Train>().TrainCost);
            inventoryInitializer.Initialize();
            subject.InitializeInventory(decorator);
        }

        private int CalculateTotalCapacity(TrainYard trainYard) => trainYard.MaxCapacity * _goodService.Goods.Count;

        private void AllowEveryGoodAsGiveAndTakeable(InventoryInitializer inventoryInitializer, GoodAmountSpecification[] inventoryCapacity)
        {
            foreach (var goodAmountSpecification in inventoryCapacity)
            {
                var storableGoodAmount = new StorableGoodAmount(StorableGood.CreateAsGivable(goodAmountSpecification.GoodId),
                    goodAmountSpecification.Amount * 2);
                inventoryInitializer.AddAllowedGood(storableGoodAmount);
            }
        }
    }
}