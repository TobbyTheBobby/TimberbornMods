using Timberborn.BaseComponentSystem;
using Timberborn.Common;
using Timberborn.InventorySystem;

namespace ChooChoo.GoodsStations
{
    public class GoodsStationSendingInventory : BaseComponent
    {
        private LimitableGoodDisallower _limitableGoodDisallower;

        public Inventory Inventory { get; private set; }

        private void Awake()
        {
            GetComponentFast<GoodsStationDistributionSettings>().SettingChanged += (_, setting) => UpdateMaximumCapacity(setting);
        }

        public void InitializeSendingInventory(Inventory inventory, LimitableGoodDisallower limitableGoodDisallower)
        {
            Asserts.FieldIsNull(this, Inventory, "Inventory");
            Inventory = inventory;
            _limitableGoodDisallower = limitableGoodDisallower;
        }

        private void UpdateMaximumCapacity(GoodsStationGoodDistributionSetting setting)
        {
            if (setting.DistributionOption == DistributionOption.Sending)
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