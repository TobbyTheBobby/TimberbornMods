using Timberborn.BaseComponentSystem;
using Timberborn.Common;
using Timberborn.InventorySystem;

namespace ChooChoo.GoodsStations
{
    public class GoodsStationReceivingInventory : BaseComponent
    {
        private LimitableGoodDisallower _limitableGoodDisallower;

        public Inventory Inventory { get; private set; }

        private void Awake()
        {
            GetComponentFast<GoodsStationDistributionSettings>().SettingChanged += (_, setting) => UpdateMaximumCapacity(setting);
        }

        public void InitializeReceivingInventory(Inventory inventory, LimitableGoodDisallower limitableGoodDisallower)
        {
            Asserts.FieldIsNull(this, Inventory, "Inventory");
            Inventory = inventory;
            _limitableGoodDisallower = limitableGoodDisallower;
        }

        private void UpdateMaximumCapacity(GoodsStationGoodDistributionSetting setting)
        {
            if (setting.DistributionOption == DistributionOption.Receiving)
            {
                _limitableGoodDisallower.SetAllowedAmount(setting.GoodId, setting.MaxCapacity);
            }
            else
            {
                _limitableGoodDisallower.SetAllowedAmount(setting.GoodId, 0);
            }
        }
    }
}