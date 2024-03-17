using System.Collections.Generic;
using Timberborn.InventorySystem;
using Timberborn.InventorySystemUI;
using UnityEngine.UIElements;

namespace TobbyTools.SpecificGoodsStockpileSystem
{
  public class SpecificGoodsStockpileFragmentInventory
  {
    private readonly InformationalRowsFactory _informationalRowsFactory;
    private Inventory _inventory;
    private ScrollView _inventoryContent;
    private readonly List<InformationalRow> _rows = new();

    public SpecificGoodsStockpileFragmentInventory(InformationalRowsFactory informationalRowsFactory) => _informationalRowsFactory = informationalRowsFactory;

    public void InitializeFragment(VisualElement root)
    {
      _inventoryContent = root.Q<ScrollView>("Content");
    }

    public void ShowFragment(Inventory inventory)
    {
      _inventory = inventory;
      foreach (var storableGoodAmount in _inventory.AllowedGoods)
        _rows.Add(_informationalRowsFactory.CreateInformationalRow(storableGoodAmount.StorableGood, _inventory, _inventoryContent, true));
    }

    public void ClearFragment()
    {
      _inventoryContent.Clear();
      _rows.Clear();
      _inventory = null;
    }

    public void UpdateFragment()
    {
      if (!_inventory || !_inventory.enabled) 
        return;
      foreach (var row in _rows)
        row.ShowUpdated();
    }
  }
}
