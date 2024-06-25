using System.Collections.Generic;
using System.Text;
using Timberborn.BaseComponentSystem;
using Timberborn.Common;
using Timberborn.CoreUI;
using Timberborn.Debugging;
using Timberborn.EntityPanelSystem;
using Timberborn.Goods;
using Timberborn.GoodsUI;
using Timberborn.InventorySystem;
using Timberborn.InventorySystemUI;
using TobbyTools.InaccessibilityUtilitySystem;
using UnityEngine.UIElements;

namespace ChooChoo.GoodsStationUI
{
    public class GoodsStationInventoryDebugFragment : IEntityPanelFragment
    {
        private readonly DebugFragmentFactory _debugFragmentFactory;
        private readonly DevModeManager _devModeManager;
        private readonly IGoodService _goodService;
        private readonly GoodDescriber _goodDescriber;
        private readonly PanelStack _panelStack;
        private readonly ModifyInventoryBox _modifyInventoryBox;
        private Label _text1;
        private Label _text2;
        private readonly List<Inventory> _inventories = new();
        private readonly StringBuilder _description = new();
        private GoodsStations.GoodsStation _goodsStation;
        private VisualElement _root;
        private VisualElement _root1;
        private VisualElement _root2;

        public GoodsStationInventoryDebugFragment(
            DebugFragmentFactory debugFragmentFactory,
            DevModeManager devModeManager,
            IGoodService goodService,
            GoodDescriber goodDescriber,
            PanelStack panelStack,
            ModifyInventoryBox modifyInventoryBox)
        {
            _debugFragmentFactory = debugFragmentFactory;
            _devModeManager = devModeManager;
            _goodService = goodService;
            _goodDescriber = goodDescriber;
            _panelStack = panelStack;
            _modifyInventoryBox = modifyInventoryBox;
        }

        public VisualElement InitializeFragment()
        {
            _root = new VisualElement();

            _root1 = _debugFragmentFactory.Create("Inventory", new DebugFragmentButton(OnModifySendingInventoryButtonClick, "Modify Inventory"));
            _text1 = _root1.Q<Label>("Text");
            _root.Add(_root1);

            _root2 = _debugFragmentFactory.Create("Inventory", new DebugFragmentButton(OnModifyReceivingInventoryButtonClick, "Modify Inventory"));
            _text2 = _root2.Q<Label>("Text");
            _root.Add(_root2);


            _root.ToggleDisplayStyle(false);

            return _root;
        }

        public void ShowFragment(BaseComponent entity)
        {
            entity.GetComponentsFast(_inventories);
            _goodsStation = entity.GetComponentFast<GoodsStations.GoodsStation>();
        }

        public void ClearFragment()
        {
            _inventories.Clear();
            UpdateContent();
            _goodsStation = null;
        }

        public void UpdateFragment()
        {
            UpdateContent();
        }

        private void OnModifySendingInventoryButtonClick()
        {
            if (!_goodsStation)
                return;

            var inventory1 = _goodsStation.SendingInventory;
            if (!(bool)(UnityEngine.Object)inventory1 && inventory1.enabled)
                return;
            _panelStack.PushOverlay(_modifyInventoryBox);
            InaccessibilityUtilities.InvokeInaccessibleMethod(_modifyInventoryBox, "Initialize", new object[] { inventory1 });
        }

        private void OnModifyReceivingInventoryButtonClick()
        {
            if (!_goodsStation)
                return;

            var inventory1 = _goodsStation.ReceivingInventory;
            if (!(bool)(UnityEngine.Object)inventory1 && inventory1.enabled)
                return;
            _panelStack.PushOverlay(_modifyInventoryBox);
            InaccessibilityUtilities.InvokeInaccessibleMethod(_modifyInventoryBox, "Initialize", new object[] { inventory1 });
        }

        private void UpdateContent()
        {
            _description.Clear();
            if (_devModeManager.Enabled && _goodsStation != null)
            {
                DescribeInventory(_goodsStation.SendingInventory, _description);
                _text1.text = _description.ToStringWithoutNewLineEnd();

                DescribeInventory(_goodsStation.ReceivingInventory, _description);
                _text2.text = _description.ToStringWithoutNewLineEnd();
            }

            _root.ToggleDisplayStyle(_description.Length > 0);
        }

        private void DescribeInventory(Inventory inventory, StringBuilder description)
        {
            description.Append(inventory.ComponentName);
            description.AppendLine(inventory.enabled ? " (on)" : " (off)");
            foreach (var good in _goodService.Goods)
                DescribeGood(inventory, good, description);
        }

        private void DescribeGood(Inventory inventory, string goodId, StringBuilder description)
        {
            var num1 = inventory.AmountInStock(goodId);
            var num2 = inventory.ReservedCapacity(goodId);
            if (num1 <= 0 && num2 <= 0)
                return;
            var num3 = num1 - inventory.UnreservedAmountInStock(goodId);
            var str = $"{_goodDescriber.Describe(goodId)}: {num1} ({num3} reserved, {num2} incoming)";
            description.AppendLine(str);
        }
    }
}