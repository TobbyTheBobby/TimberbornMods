using Timberborn.BaseComponentSystem;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using UnityEngine.UIElements;

namespace TobbyTools.SpecificGoodsStockpileSystem
{
    internal class SpecificGoodsStockpileInventoryFragment : IEntityPanelFragment
    {
        private readonly SpecificGoodsStockpileFragmentInventory _specificGoodsStockpileFragmentInventory;
        private readonly VisualElementLoader _visualElementLoader;
        private SpecificGoodsStockpile _specificGoodsStockpile;
        private VisualElement _root;

        public SpecificGoodsStockpileInventoryFragment(
            SpecificGoodsStockpileFragmentInventory specificGoodsStockpileFragmentInventory,
            VisualElementLoader visualElementLoader)
        {
            _specificGoodsStockpileFragmentInventory = specificGoodsStockpileFragmentInventory;
            _visualElementLoader = visualElementLoader;
        }

        public VisualElement InitializeFragment()
        {
            _root = _visualElementLoader.LoadVisualElement("Game/EntityPanel/ConstructionSiteFragment");
            _root.Q<Timberborn.CoreUI.ProgressBar>("ProgressBar").ToggleDisplayStyle(false);
            _root.Q<Label>("Text").ToggleDisplayStyle(false);
            _root.Q<VisualElement>("HeaderWrapper").ToggleDisplayStyle(false);
            _root.ToggleDisplayStyle(false);
            _specificGoodsStockpileFragmentInventory.InitializeFragment(_root);
            return _root;
        }

        public void ShowFragment(BaseComponent entity)
        {
            _specificGoodsStockpile = entity.GetComponentFast<SpecificGoodsStockpile>();
            if (!_specificGoodsStockpile)
                return;
            _specificGoodsStockpileFragmentInventory.ShowFragment(_specificGoodsStockpile.Inventory);
        }

        public void ClearFragment()
        {
            _specificGoodsStockpile = null;
            _specificGoodsStockpileFragmentInventory.ClearFragment();
            _root.ToggleDisplayStyle(false);
        }

        public void UpdateFragment()
        {
            if (!_specificGoodsStockpile || !_specificGoodsStockpile.enabled)
            {
                _root.ToggleDisplayStyle(false);
            }
            else
            {
                _root.ToggleDisplayStyle(true);
                _specificGoodsStockpileFragmentInventory.UpdateFragment();
            }
        }
    }
}