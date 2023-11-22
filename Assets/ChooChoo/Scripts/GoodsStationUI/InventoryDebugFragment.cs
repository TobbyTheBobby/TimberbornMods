using System.Collections.Generic;
using System.Text;
using ChooChoo;
using Timberborn.BaseComponentSystem;
using Timberborn.Common;
using Timberborn.CoreUI;
using Timberborn.Debugging;
using Timberborn.EntityPanelSystem;
using Timberborn.Goods;
using Timberborn.GoodsUI;
using Timberborn.InventorySystem;
using UnityEngine.UIElements;

namespace Timberborn.InventorySystemUI
{
  public class GoodsStationInventoryDebugFragment : IEntityPanelFragment
  {
    public readonly DebugFragmentFactory _debugFragmentFactory;
    public readonly DevModeManager _devModeManager;
    public readonly IGoodService _goodService;
    public readonly GoodDescriber _goodDescriber;
    public readonly PanelStack _panelStack;
    public readonly ModifyInventoryBox _modifyInventoryBox;
    public Label _text1;
    public Label _text2;
    public readonly List<Inventory> _inventories = new();
    public readonly StringBuilder _description = new();
    public GoodsStation _goodsStation;
    public VisualElement _root;
    public VisualElement _root1;
    public VisualElement _root2;

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
      _goodsStation = entity.GetComponentFast<GoodsStation>();
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

    public void OnModifySendingInventoryButtonClick()
    {
      if (!_goodsStation)
        return;

      Inventory inventory1 = _goodsStation.SendingInventory;
      if (!(bool) (UnityEngine.Object) inventory1 && inventory1.enabled)
        return;
      _panelStack.PushOverlay(_modifyInventoryBox);
      ChooChooCore.InvokePrivateMethod(_modifyInventoryBox, "Initialize", new object[] { inventory1 });
    }
    
    public void OnModifyReceivingInventoryButtonClick()
    {
      if (!_goodsStation)
        return;

      Inventory inventory1 = _goodsStation.ReceivingInventory;
      if (!(bool) (UnityEngine.Object) inventory1 && inventory1.enabled)
        return;
      _panelStack.PushOverlay(_modifyInventoryBox);
      ChooChooCore.InvokePrivateMethod(_modifyInventoryBox, "Initialize", new object[] { inventory1 });
    }

    public void UpdateContent()
    {
      _description.Clear();
      if (_devModeManager.Enabled && _goodsStation != null)
      {
        DescribeInventory(_goodsStation.SendingInventory, _description);
        _text1.text = _description.ToStringWithoutNewLineEnd();
        
        DescribeInventory(_goodsStation.ReceivingInventory, _description);
        _text2.text = _description.ToStringWithoutNewLineEnd();
      }

      if (_description.Length > 0)
        _root.ToggleDisplayStyle(true);
      else
        _root.ToggleDisplayStyle(false);
    }

    private void DescribeInventory(Inventory inventory, StringBuilder description)
    {
      description.Append(inventory.ComponentName);
      description.AppendLine(inventory.enabled ? " (on)" : " (off)");
      foreach (string good in _goodService.Goods)
        DescribeGood(inventory, good, description);
    }

    private void DescribeGood(Inventory inventory, string goodId, StringBuilder description)
    {
      int num1 = inventory.AmountInStock(goodId);
      int num2 = inventory.ReservedCapacity(goodId);
      if (num1 <= 0 && num2 <= 0)
        return;
      int num3 = num1 - inventory.UnreservedAmountInStock(goodId);
      string str = $"{_goodDescriber.Describe(goodId)}: {num1}" + $" ({num3} reserved, {num2} incoming)";
      description.AppendLine(str);
    }
  }
}
