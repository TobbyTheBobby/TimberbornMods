// Decompiled with JetBrains decompiler
// Type: Timberborn.Reproduction.BreedingPodInventoryInitializer
// Assembly: Timberborn.Reproduction, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50A81F73-BABD-462C-95E2-88FDEFF56003
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Timberborn\Timberborn_Data\Managed\Timberborn.Reproduction.dll

using System.Collections.Generic;
using Timberborn.Goods;
using Timberborn.InventorySystem;
using Timberborn.TemplateSystem;

namespace PlantingSeeds
{
  public class SeedReceiverInventoryInitializer : IDedicatedDecoratorInitializer<SeedReceiver, Inventory>
  {
    private static readonly string InventoryComponentName = "SeedReceiver";
    private readonly IGoodService _goodService;
    private readonly InventoryInitializerFactory _inventoryInitializerFactory;

    public SeedReceiverInventoryInitializer(
      IGoodService goodService,
      InventoryInitializerFactory inventoryInitializerFactory)
    {
      _goodService = goodService;
      _inventoryInitializerFactory = inventoryInitializerFactory;
    }

    public void Initialize(SeedReceiver subject, Inventory decorator)
    {
      List<StorableGoodAmount> storableGoodAmountList = new List<StorableGoodAmount>();
      foreach (var good in _goodService.Goods)
      {
        StorableGood asGivable = StorableGood.CreateAsGivable(good);
        storableGoodAmountList.Add(new StorableGoodAmount(asGivable, 100000));
      }
      InventoryInitializer inventoryInitializer = _inventoryInitializerFactory.CreateWithUnlimitedCapacity(decorator, InventoryComponentName);
      inventoryInitializer.AddAllowedGoods(storableGoodAmountList);
      inventoryInitializer.Initialize();
      subject.InitializeInventory(decorator);
    }
  }
}
