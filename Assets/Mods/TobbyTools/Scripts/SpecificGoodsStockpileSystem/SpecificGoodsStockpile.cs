using Timberborn.BaseComponentSystem;
using Timberborn.Common;
using Timberborn.ConstructibleSystem;
using Timberborn.EntitySystem;
using Timberborn.Goods;
using Timberborn.InventorySystem;
using UnityEngine;

namespace TobbyTools.SpecificGoodsStockpileSystem
{
    public class SpecificGoodsStockpile : BaseComponent, IRegisteredComponent, IFinishedStateListener
    {
        [SerializeField]
        public GoodAmountSpecification[] _storableGoods;

        public Inventory Inventory { get; private set; }

        public GoodAmountSpecification[] StorableGoods => _storableGoods;

        public void Awake()
        {
            enabled = false;
        }

        public void OnEnterFinishedState()
        {
            enabled = true;
            Inventory.Enable();
        }

        public void OnExitFinishedState()
        {
            Inventory.Disable();
            enabled = false;
        }

        public void InitializeInventory(Inventory inventory)
        {
            Asserts.FieldIsNull(this, Inventory, "Inventory");
            Inventory = inventory;
        }
    }
}