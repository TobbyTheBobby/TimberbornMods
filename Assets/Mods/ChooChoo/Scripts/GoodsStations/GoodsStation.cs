using Timberborn.BaseComponentSystem;
using Timberborn.BuildingsBlocking;
using Timberborn.ConstructibleSystem;
using Timberborn.EntitySystem;
using Timberborn.InventorySystem;

namespace ChooChoo.GoodsStations
{
    public class GoodsStation : BaseComponent, IRegisteredComponent, IFinishedStateListener, IPausableComponent
    {
        public static readonly int Capacity = 200;

        private GoodsStationReceivingInventory _goodsStationReceivingInventory;
        private GoodsStationSendingInventory _goodsStationSendingInventory;

        public void Awake()
        {
            _goodsStationReceivingInventory = GetComponentFast<GoodsStationReceivingInventory>();
            _goodsStationSendingInventory = GetComponentFast<GoodsStationSendingInventory>();
            enabled = false;
        }

        public void OnEnterFinishedState()
        {
            enabled = true;
            SendingInventory.Enable();
            ReceivingInventory.Enable();
        }

        public void OnExitFinishedState()
        {
            SendingInventory.Disable();
            ReceivingInventory.Disable();
            enabled = false;
        }

        public Inventory SendingInventory => _goodsStationSendingInventory.Inventory;

        public Inventory ReceivingInventory => _goodsStationReceivingInventory.Inventory;
    }
}