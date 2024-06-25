using ChooChoo.GoodsStations;
using Timberborn.BaseComponentSystem;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.InventorySystemUI;
using UnityEngine.UIElements;

namespace ChooChoo.GoodsStationUI
{
    internal class GoodsStationReceivingInventoryFragment : IEntityPanelFragment
    {
        private readonly InventoryFragmentBuilderFactory _inventoryFragmentBuilderFactory;
        private readonly VisualElementLoader _visualElementLoader;
        private InventoryFragment _inventoryFragment;
        private GoodsStation _goodsStation;
        private VisualElement _root;

        public GoodsStationReceivingInventoryFragment(
            InventoryFragmentBuilderFactory inventoryFragmentBuilderFactory,
            VisualElementLoader visualElementLoader)
        {
            _inventoryFragmentBuilderFactory = inventoryFragmentBuilderFactory;
            _visualElementLoader = visualElementLoader;
        }

        public VisualElement InitializeFragment()
        {
            _root = _visualElementLoader.LoadVisualElement("Game/EntityPanel/DistrictCrossingInventoryFragment");
            _root.ToggleDisplayStyle(false);
            _inventoryFragment = _inventoryFragmentBuilderFactory.CreateBuilder(_root).ShowNoGoodInStockMessage().Build();
            return _root;
        }

        public void ShowFragment(BaseComponent entity)
        {
            _goodsStation = entity.GetComponentFast<GoodsStation>();
            if (!_goodsStation)
                return;
            _root.ToggleDisplayStyle(true);
            _inventoryFragment.ShowFragment(_goodsStation.ReceivingInventory);
        }

        public void ClearFragment()
        {
            _goodsStation = null;
            _inventoryFragment.ClearFragment();
            _root.ToggleDisplayStyle(false);
        }

        public void UpdateFragment()
        {
            if (!_goodsStation)
                return;
            _inventoryFragment.UpdateFragment();
        }
    }
}