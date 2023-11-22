using Bindito.Core;
using Timberborn.BlockSystem;
using Timberborn.Common;
using Timberborn.ConstructibleSystem;
using Timberborn.EntitySystem;
using Timberborn.Goods;
using Timberborn.InventorySystem;
using Timberborn.TickSystem;
using TobbyTools.InaccessibilityUtilitySystem;

namespace PlantingSeeds
{
    public class SeedReceiver : TickableComponent, IFinishedStateListener
    {
        private EntityService _entityService;
        
        private BlockObject _blockObject;
        private GoodRegistry _reservedCapacity;
        
        public Inventory Inventory { get; private set; }

        [Inject]
        public void InjectDependencies(EntityService entityService)
        {
            _entityService = entityService;
        }
        
        private void Awake()
        {
            _blockObject = GetComponentFast<BlockObject>();
        }

        public void InitializeInventory(Inventory inventory)
        {
            Asserts.FieldIsNull(this, Inventory, "Inventory");
            Inventory = inventory;
            _reservedCapacity = (GoodRegistry)InaccessibilityUtilities.GetInaccessibleField(Inventory, "_reservedCapacity");
        }
        
        public override void Tick()
        {
            Plugin.Log.LogInfo(_reservedCapacity.TotalAmount + "");
            if (Inventory.enabled || _reservedCapacity.TotalAmount > 0)
                return;

            _entityService.Delete(_blockObject);
        }

        public void OnEnterFinishedState()
        {
            Inventory.Enable();
        }

        public void OnExitFinishedState()
        {
            Inventory.Disable();
        }
    }
}