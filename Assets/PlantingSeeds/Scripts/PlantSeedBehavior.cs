using System;
using System.Linq;
using Bindito.Core;
using Timberborn.AssetSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.BehaviorSystem;
using Timberborn.BlockSystem;
using Timberborn.Buildings;
using Timberborn.Carrying;
using Timberborn.Common;
using Timberborn.ConstructionSites;
using Timberborn.Coordinates;
using Timberborn.EntitySystem;
using Timberborn.GameDistricts;
using Timberborn.Goods;
using Timberborn.InventorySystem;
using Timberborn.Navigation;
using Timberborn.Planting;
using Timberborn.PrefabSystem;
using Timberborn.WorkSystem;
using UnityEngine;

namespace PlantingSeeds
{
    public class PlantSeedBehavior : BaseComponent
    {
        private PlantableSeedSpecificationApplier _plantableSeedSpecificationApplier;
        private IResourceAssetLoader _resourceAssetLoader;
        private ConstructionFactory _constructionFactory;
        private PrefabNameMapper _prefabNameMapper;
        private PlantingService _plantingService;
        private BlockService _blockService;

        private CarrierInventoryFinder _carrierInventoryFinder;
        private CarryAmountCalculator _carryAmountCalculator;
        private GoodReserver _goodReserver;
        private GoodCarrier _goodCarrier;
        private Planter _planter;
        private Worker _worker;

        private Building SeedsPrefab;

        [Inject]
        public void InjectDependencies(PlantableSeedSpecificationApplier plantableSeedSpecificationApplier, CarryAmountCalculator carryAmountCalculator, IResourceAssetLoader resourceAssetLoader, ConstructionFactory blockObjectFactory, PrefabNameMapper prefabNameMapper, PlantingService plantingService, BlockService blockService)
        {
            _plantableSeedSpecificationApplier = plantableSeedSpecificationApplier;
            _carryAmountCalculator = carryAmountCalculator;
            _resourceAssetLoader = resourceAssetLoader;
            _constructionFactory = blockObjectFactory;
            _prefabNameMapper = prefabNameMapper;
            _plantingService = plantingService;
            _blockService = blockService;
        }

        private void Awake()
        {
            _carrierInventoryFinder = GetComponentFast<CarrierInventoryFinder>();
            _goodReserver = GetComponentFast<GoodReserver>();
            _goodCarrier = GetComponentFast<GoodCarrier>();
            _planter = GetComponentFast<Planter>();
            _worker = GetComponentFast<Worker>();
            SeedsPrefab = _resourceAssetLoader.Load<GameObject>("tobbert.plantingseeds/tobbert_plantingseeds/Seeds").GetComponent<Building>();
        }

        public bool Decide(out Decision decision)
        {
            Plugin.Log.LogInfo("Trying to decide");
            decision = Decision.ReleaseNow();
            if (!_planter.PlantingCoordinates.HasValue)
            {
                return true;
            }

            var coordinates = _planter.PlantingCoordinates.Value;
            var blockObject = _blockService.GetBottomObjectAt(coordinates);
            if (blockObject != null && blockObject.TryGetComponentFast(out SeedReceiver seedReceiver) && seedReceiver.Inventory.Stock.Sum(goodAmount => goodAmount.Amount) > 0)
            {
                return true;
            }
                
            var plantId = _plantingService.GetResourceAt(coordinates.XY());
            if (plantId == null)
                return false;
            var plantingSeedComponent = _prefabNameMapper.GetPrefab(plantId).GetComponentFast<PlantingSeedComponent>();
            if (plantingSeedComponent == null)
                return true;
            var receivingInventory = _constructionFactory.CreateAsFinished(SeedsPrefab, coordinates, Orientation.Cw0).GetComponentFast<SeedReceiver>().Inventory;
            receivingInventory.GetComponentFast<EntityComponent>().Start();
            // Plugin.Log.LogInfo((_carrierInventoryFinder == null) + "");
            // Plugin.Log.LogInfo((plantingSeedComponent == null) + "");
            // Plugin.Log.LogInfo((receivingInventory == null) + "");
            var givingInventory = _worker.Workplace.GetComponentFast<DistrictBuilding>().District.GetComponentFast<DistrictInventoryPicker>().ClosestInventoryWithStock(_worker.Workplace.GetComponentFast<Accessible>(), plantingSeedComponent.GoodId, _ => true);
            
            // if (_carrierInventoryFinder.TryCarryFromAnyInventoryLimited(plantingSeedComponent.GoodId, inventory, plantingSeedComponent.GoodAmount))
            if (givingInventory != null && TryReserveInventories(plantingSeedComponent.GoodId, receivingInventory, givingInventory, plantingSeedComponent.GoodAmount))
            {
                decision = Decision.ReleaseNextTick();
                return false;
            }
            
            return false;
        }
        
        private bool TryReserveInventories(
            string goodId,
            Inventory receivingInventory,
            Inventory givingInventory,
            int? maxAmount)
        {
            GoodAmount carriableGood = GetCarriableGood(goodId, receivingInventory, givingInventory, maxAmount);
            if (carriableGood.Amount <= 0)
                return false;
            if (maxAmount.HasValue)
                _goodReserver.ReserveExactStockAmount(givingInventory, carriableGood);
            else
                _goodReserver.ReserveNotLessThanStockAmount(givingInventory, carriableGood);
            _goodReserver.ReserveCapacity(receivingInventory, carriableGood);
            return true;
        }
        
        private GoodAmount GetCarriableGood(
            string goodId,
            Inventory receivingInventory,
            Inventory givingInventory,
            int? maxAmount)
        {
            GoodAmount carry = _carryAmountCalculator.AmountToCarry(_goodCarrier.LiftingCapacity, goodId, receivingInventory, givingInventory);
            if (maxAmount.HasValue)
            {
                int amount = carry.Amount;
                int? nullable = maxAmount;
                int valueOrDefault = nullable.GetValueOrDefault();
                if (amount > valueOrDefault & nullable.HasValue)
                    return new GoodAmount(goodId, maxAmount.Value);
            }
            return carry;
        }
    }
}